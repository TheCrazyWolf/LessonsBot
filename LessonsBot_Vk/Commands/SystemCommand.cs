using LessonsBot_DB.ModelsDb;
using LessonsBot_DB.ModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.GroupUpdate;

namespace LessonsBot_Vk.Commands
{
    internal class SystemCommand
    {
        VkApi _api;
        Bot _bot;
        Message _message;
        DbProvider _db;
        string[] msg_array;

        public SystemCommand(IGroupUpdate item, ref VkApi vkApi, ref Bot bot)
        {
            _message = (Message) item;
            _api = vkApi;
            _db = new DbProvider();
            _bot = bot;

            SLogger.Write($"-----------------------------------");
            SLogger.Write($"[{_bot.IdBot}] Новое сообщение:");
            SLogger.Write($"Отправитель: {_message.FromId}");
            SLogger.Write($"Беседа: {_message.PeerId}");
            SLogger.Write($"Содержимое: {_message.Text}");


            msg_array = new Regex("\\[.*\\][\\s,]*").Replace(_message.Text.ToLower(), "").Split(" ");


            switch (msg_array[0].ToLower())
            {
                case "!помощь":
                case "!хелп":
                case "!help":
                    Help();
                    break;
                case "!привязать":
                case "!подписаться":
                case "!подписать":
                case "!подписка":
                    Bind();
                    break;
                case "!очистка":
                    UnBind();
                    break;
                default:
                    break;
            }

        }

        private void Bind()
        {
            if (_bot.PeerProps.Count >= 5)
            {
                _api.Messages.Send(new() { Message = "Дружок, уже перебор... " +
                    "На эту беседу привязано больше 5 настроек!", 
                    PeerId = _message.PeerId, RandomId = new Random().Next() });
                return;
            }

            string[] msg_array = _message.Text.Split(' ');

            if (msg_array.Length <= 1)
            {
                _api.Messages.Send(new() { Message = "Недостаточно аргументов", 
                    PeerId = _message.PeerId, RandomId = new Random().Next() });
                return;
            }

            var find_teachers = _db.TeacherCaches.ToList()
                .FirstOrDefault(x => x.id == msg_array[1] || x.name.ToLower() == msg_array[1].ToLower());

            var find_groupropa = _db.GroupsCache.ToList()
                .FirstOrDefault(x => x.Id.ToString() == msg_array[1] || x.Name.ToLower() == msg_array[1].ToLower());

            if (find_groupropa == null)
            {
                _api.Messages.Send(new() { Message = "Не удалось получить список преподов/групп", 
                    PeerId = _message.PeerId, RandomId = new Random().Next() });
                return;
            }

            PeerProp prop = new PeerProp();

            if (find_teachers != null)
            {
                prop.TypeLesson = TypeLesson.Teacher;
                prop.Value = find_teachers.id;
            }

            if (find_groupropa != null)
            {
                prop.TypeLesson = TypeLesson.Group;
                prop.Value = find_groupropa.Id.ToString();
            }

            prop.IdPeer = (long)_message.PeerId;

            _bot.PeerProps.Add(prop);

            _api.Messages.Send(new() { Message = "Привязано!", 
                PeerId = _message.PeerId, RandomId = new Random().Next() });
        }
        private string UnBind()
        {
            foreach (var item in _bot.PeerProps.ToList().Where(x=> x.IdPeer == _message.PeerId))
            {
                _bot.PeerProps.Remove(item);
            }

            return $"В беседе #{_message.PeerId} были очищены привязки";
        }
        private string Help()
        {
            return $"Меню в разработке";
        }

    }
}
