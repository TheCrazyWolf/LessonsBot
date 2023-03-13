using LessonsBot_DB.ModelsDb;
using LessonsBot_DB.ModelService;
using LessonsBot_Vk.Commands;
using Microsoft.EntityFrameworkCore;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace LessonsBot_Vk
{
    internal class VkService
    {
        protected DbProvider _db;

        /* Текущий бот */
        protected Bot _bot;

        /* Счётчик ошибок */
        protected int _err = 0;

        /* ВКшная библиотка */
        protected VkApi _vkApi;

        /* Поток LongPoll и листы с задачами */
        protected Thread _longpollThread;
        protected Thread _taskScheduler;

        public delegate void _VkServiceHandler();
        public event _VkServiceHandler NotifyVkServicesProps;

        public VkService(Bot bot, ref DbProvider db)
        {
            _bot = bot;
            _db = db;
        }

        public void Start()
        {            
            string[] info =
            {
                $"-------------------------------------------",
                $"Индентификатор бота {_bot.IdBot}",
                $"Токен ***{_bot.Token.Substring(_bot.Token.Length - 10)}",
                $"Ожидание {_bot.TimeOutResponce} мс",
                $"Индентификатор сообщества {_bot.IdValueService}",
                $"Подключенных бесед {_bot.PeerProps.Count}",
                $"-------------------------------------------",
            };
            
            foreach (var item in info)
            {
                SLogger.Write(item);
            }

            _vkApi = new VkApi();
            _vkApi.Authorize(new ApiAuthParams() { AccessToken = _bot.Token });


            NotifyVkServicesProps += VkService_NotifyVkServicesProps;

            _longpollThread = new Thread(() => ThreadBootLongPoll());
            _longpollThread.Start();

            _taskScheduler = new Thread(() => ThreadTaskSheduler());
            _taskScheduler.Start();
        }

        protected void ThreadBootLongPoll()
        {
            SLogger.WriteWarning($"[{_bot.IdBot}] Подключаемся к LongPoll");

            while (true)
            {
                if (IsDeadBot())
                    break;

                try
                {
                    var s = _vkApi.Groups.GetLongPollServer((ulong)_bot.IdValueService);
                    var poll = _vkApi.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams()
                    {
                        Server = s.Server,
                        Key = s.Key,
                        Ts = s.Ts,
                        Wait = 25
                    });

                    if (poll?.Updates == null) continue;

                    foreach (var item in poll?.Updates)
                    {
                        if (item.Message.Text[0] == '!')
                        {
                            new SystemCommand(item.Instance, ref _vkApi, ref _db, ref _bot);
                            NotifyVkServicesProps?.Invoke();
                        }
                        else
                        {

                        }
                    }

                    
                }
                catch (Exception ex)
                {
                    SLogger.WriteDanger($"[{_bot.IdBot}] {ex.Message}");
                    _err++;
                }

            }
        }

        protected void ThreadTaskSheduler()
        {
            while (true)
            {
                if (IsDeadBot())
                    break;

                _bot = _db.Bots.Include(x => x.PeerProps).FirstOrDefault(x => x.IdBot == _bot.IdBot);

                Thread.Sleep(_bot.TimeOutResponce);
                if (DateTime.Now.Hour >= 22 || DateTime.Now.Hour <= 10)
                    continue;

                foreach (var item in _bot.PeerProps)
                {
                    SLogger.Write($"[{_bot.IdBot}] Выполняется задача #{item.IdPeerProp} с типом {item.TypeLesson} для беседы {item.IdPeer} со значением {item.Value}");
                    try
                    {
                       /* Новое расписание */
                        var responce = ApiSgk
                            .GetLessons(item.TypeLesson, DateTime.Now.AddDays(1), item.Value);

                        /* Хеш расписания чтобы сравнить менялось ли оно */
                        string md5 = responce.GetMD5();

                        /* Если не изменилось пропускаем */
                        //if (item.LastResult == md5)
                        //{
                        //    SLogger.Write($"[{_bot.IdBot}] #{item.IdPeerProp} расписание не изменилось! Пропускаем");
                        //    continue;
                        //}

                        /* Составляем строчку! */
                        //string message = "";

                        //switch (item.TypeLesson)
                        //{
                        //    case TypeLesson.Group:
                        //        message = $"Расписание на {responce.date} для группы {_db.GroupsCache
                        //            .FirstOrDefault(x => x.Id.ToString() == item.Value).Name}\n";
                        //        break;
                        //    case TypeLesson.Teacher:
                        //        message = $"Расписание на {responce.date} для преподавателя {_db.TeacherCaches
                        //            .FirstOrDefault(x => x.id.ToString() == item.Value).name}\n";
                        //        break;
                        //    case TypeLesson.Cabinet:
                        //        message = $"Расписание на {responce.date} для кабинета {item.Value}\n";
                        //        break;
                        //}


                        string lessons_builder = responce.BuilderString();

                        /* Если пустые уроки нехрен отправлять! */
                        if (string.IsNullOrEmpty(lessons_builder))
                            continue;

                        //SLogger.Write($"[{_bot.IdBot}] #{item.IdPeerProp} отправка в беседу {message + lessons_builder}");

                        _vkApi.Messages.Send(new()
                        {
                            PeerId = item.IdPeer,
                            Message = lessons_builder,
                            RandomId = new Random().Next()
                        });

                        /* Назначаем полученный хеш чтобы в будущем сравнить! */
                        item.LastResult = md5;

                        _db.Update(item);
                        _db.SaveChanges();

                        NotifyVkServicesProps?.Invoke();

                    }
                    catch (Exception ex)
                    {
                        SLogger.WriteDanger($"[{_bot.IdBot}] {ex.Message}");
                        _err++;
                    }
                }

            }
        }


        /* Проверка живой ли бот */
        protected bool IsDeadBot()
        {
            if (_err >= 10)
            {
                SLogger.WriteDanger($"[{_bot.IdBot}] Количество ошибок {_err}");
                SLogger.WriteWarning($"[{_bot.IdBot}] Бот отключен");
                return true;
            }

            return false;
        }

        protected void VkService_NotifyVkServicesProps()
        {
            SLogger.Write($"[{_bot.IdBot}] Сохранение изменений");
            try
            {
                _db.Update(_bot);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                SLogger.WriteDanger($"[{_bot.IdBot}] {ex.Message}");
            }
        }

    }
}
