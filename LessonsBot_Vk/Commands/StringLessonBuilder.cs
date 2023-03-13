using LessonsBot_DB.ModelsDb;
using LessonsBot_DB.ModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LessonsBot_Vk.Commands
{
    internal class StringLessonBuilder
    {
        public static string Builder(PeerProp prop, DateTime date)
        {
            PeerProp _prop = new DbProvider().PeerProps
                .FirstOrDefault(x => x.IdPeerProp == prop.IdPeerProp);
            if (prop == null)
                return "";

            string result = "";

            if(prop.TypeLesson == TypeLesson.Group)
            {
                var date_next = ApiSgk
                    .GetLessons(prop.TypeLesson, date, prop.Value);
                var group = new DbProvider().GroupsCache
                    .FirstOrDefault(x => x.Id.ToString() == prop.Value);

                if (date_next.lessons.Count == 0)
                    return "";

                result = $"Расписание на {date_next.date} для группы {group.Name}\n";

                if (date.DayOfWeek == DayOfWeek.Monday)
                    result += $"\n8.25 \nКлассный час \n{FioMiniWork(date_next.lessons[0].teachername)} \n{date_next.lessons[0].cab}\n";
                
                foreach (var item2 in date_next.lessons)
                {
                    result += $"\n{item2.num}. \n{item2.title} \n{FioMiniWork(item2.teachername)} \n{item2.cab}\n";
                }
            }


            new DbProvider().Update(prop);
            new DbProvider().SaveChanges();

            return result;
        }

        private static string FioMiniWork(string fio)
        {
            fio = Regex.Replace(fio, @"\s+", " ");

            string[] fio_array = fio.Split(' ');

            if (fio_array.Length >= 2)
                return $"{fio_array[0]} {fio_array[1][0]}. {fio_array[2][0]}.";

            return fio;
        } 
    }

}
