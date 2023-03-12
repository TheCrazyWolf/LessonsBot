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
        public static string Builder(ref PeerProp prop, DateTime date)
        {
            string result = "";
            if(prop.TypeLesson == TypeLesson.Group)
            {
                var date_next = ApiSgk.GetLessons(prop.TypeLesson, date, prop.Value);

                result = $"Расписание на {date_next.date} \n";

                if (date.DayOfWeek == DayOfWeek.Monday && date_next.lessons.Count != 0)
                {
                    if (date_next.lessons[0].num == "1")
                        result += $"0. 8.25 Классный час (Разговоры о важном) {FioMiniWork(date_next.lessons[0].teachername)} {date_next.lessons[0].cab}";

                    foreach (var item2 in date_next.lessons)
                    {
                        result += $"\n{item2.num}. {item2.title} {FioMiniWork(item2.teachername)} {item2.cab}";
                    }

                }
            }
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
