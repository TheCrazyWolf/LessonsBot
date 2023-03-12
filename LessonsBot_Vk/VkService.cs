using LessonsBot_DB.ModelsDb;
using LessonsBot_Vk.Commands;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace LessonsBot_Vk
{
    internal class VkService
    {
        protected DbProvider _db;
        protected Bot _bot;
        protected int _err = 0;

        protected VkApi _vkApi;

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
                $"Токен {_bot.Token}",
                $"Ожидание {_bot.TimeOutResponce} мс",
                $"Индетификатор сообщества {_bot.IdValueService}",
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
                       new Thread( new SystemCommand(item, ref this) ).Start();
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


                Thread.Sleep(_bot.TimeOutResponce);
                if (DateTime.Now.Hour >= 22 || DateTime.Now.Hour <= 10)
                    continue;

                foreach (var item in _bot.PeerProps)
                {
                    try
                    {
                        //_vkApi.Messages.Send(new() { PeerId = item.IdPeer, Message = })
                    }
                    catch (Exception ex )
                    {
                        SLogger.WriteDanger($"[{_bot.IdBot}] {ex.Message}");
                        _err++;
                    }
                }

            }
        }

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

        private void VkService_NotifyVkServicesProps()
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
