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

        public VkService(Bot bot)
        {
            _bot = bot;
            Start();
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

        /* Слушаем LongPoll */
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
                        new MessageEvent(item.Instance, ref _vkApi, ref _bot);
                        NotifyVkServicesProps?.Invoke();
                    }

                    
                }
                catch (Exception ex)
                {
                    SLogger.WriteDanger($"[{_bot.IdBot}] {ex.Message}");
                    _err++;
                }

            }
        }

        /* Выполняем таски по расписанию */
        protected void ThreadTaskSheduler()
        {
            while (true)
            {
                if (IsDeadBot())
                    break;

                /* Актуализируем бота по настройкам! */
                _bot = new DbProvider().Bots.Include(x => x.PeerProps)
                    .FirstOrDefault(x => x.IdBot == _bot.IdBot);

                Thread.Sleep(_bot.TimeOutResponce);

                /* Не долбить API ночью */
                if (DateTime.Now.Hour >= 22 || DateTime.Now.Hour <= 10)
                    continue;

                foreach (var item in _bot.PeerProps)
                {
                    SLogger.Write($"[{_bot.IdBot}] Выполняется задача #{item.IdPeerProp} с типом {item.TypeLesson} для беседы {item.IdPeer} со значением {item.Value}");
                    try
                    {
                        /* Новое расписание */
                        ApiLessons responce = null;

                        /* Специальное расписание если пятница ? */
                        if(DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                        {
                            responce = ApiSgk
                            .GetLessons(item.TypeLesson, DateTime.Now.AddDays(3), item.Value);
                        }
                        else /* Стандартное расписание на день вперед */
                        {
                            responce = ApiSgk
                            .GetLessons(item.TypeLesson, DateTime.Now.AddDays(1), item.Value);
                        }
                        
                        /* Хеш расписания чтобы сравнить менялось ли оно */
                        string md5 = responce.GetMD5();

                        /* Если не изменилось пропускаем */
                        if (item.LastResult == md5)
                        {
                            SLogger.Write($"[{_bot.IdBot}] #{item.IdPeerProp} расписание не изменилось! Пропускаем");
                            continue;
                        }

                        //SLogger.Write($"[{_bot.IdBot}] #{item.IdPeerProp} отправка в беседу {message + lessons_builder}");

                        _vkApi.Messages.Send(new()
                        {
                            PeerId = item.IdPeer,
                            Message = responce.BuilderString(),
                            RandomId = new Random().Next()
                        });

                        /* Назначаем полученный хеш чтобы в будущем сравнить! */
                        item.LastResult = md5;

                        using (DbProvider _ef = new DbProvider())
                        {
                            _ef.Update(item);
                            _ef.SaveChanges();
                        }

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
            SLogger.Write($"[{_bot.IdBot}] Обновление параметров");
            try
            {
                using(DbProvider _ef = new DbProvider())
                {
                    _ef.Update(_bot);
                    _ef.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                SLogger.WriteDanger($"[{_bot.IdBot}] {ex.Message}");
            }
        }

    }
}
