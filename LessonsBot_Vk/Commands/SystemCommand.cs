using LessonsBot_DB.ModelsDb;
using LessonsBot_DB.ModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public SystemCommand(GroupUpdate message, ref VkService vk)
        {
            //_message = (Message)message.Message;
            //_api = vk;
            //_bot = bot;

            //SLogger.Write($"-----------------------------------");
            //SLogger.Write($"[{_bot.IdBot}] Новое сообщение:");
            //SLogger.Write($"Отправитель: {_message.FromId}");
            //SLogger.Write($"Беседа: {_message.PeerId}");
            //SLogger.Write($"Содержимое: {_message.Text}");

            
        }

        public string GenerateResponce(string message)
        {
            var array_msg = message.Split(' ');

            if (message == "!расписание")
                return ApiSgk.GetLessons(TypeLesson.Teacher, new DateOnly(2023, 03, 13), 1468.ToString()).ToString();

            if (array_msg[0] == "!поиск")
            {
                return Bind(array_msg[1]);
            }


            return null;
        }

        private string Bind(string value)
        {
            ApiTeacher find_teachers = ApiSgk.GetTeachers().FirstOrDefault(x => x.id == value || x.name.ToLower() == value.ToLower());
            ApiGroups find_groupropa = ApiSgk.GetGroups().FirstOrDefault(x => x.Id.ToString() == value || x.Name.ToLower() == value.ToLower());


            if (find_groupropa != null)
                return find_groupropa.Name;

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

            return "";
        }
    }
}
