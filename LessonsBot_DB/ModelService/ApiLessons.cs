
using System.Text.RegularExpressions;

namespace LessonsBot_DB.ModelService
{
    [Serializable]
    public class ApiLessons
    {
        public string date { get; set; }
        public List<Lesson> lessons { get; set; }

        public ApiLessons()
        {
            lessons = new();
        }

        /* */
        public string BuilderString()
        {
            if (lessons.Count == 0)
                return "";

            DbProvider db = new DbProvider();


            DateTime dt = DateTime.Parse(date);

            string result = $"Расписание на {date}";

            if (dt.DayOfWeek == DayOfWeek.Monday)
                result += $"\n8.25 \nКлассный час \n{FioMiniWork(lessons[0].teachername)} \n{lessons[0].cab}\n";

            foreach (var item2 in lessons)
            {
                result += $"\n{item2.num}. \n{item2.title} \n{FioMiniWork(item2.teachername)}{item2.nameGroup} \n{item2.cab}\n";
            }

            return result;
        }
        
        public string GetMD5()
        {
            string input = "";

            foreach (var item in lessons)
            {
                input += $"{item.num} {item.title} {item.nameGroup} {item.teachername} {item.cab}";
            }

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes); // .NET 5 +
                                                       // Convert the byte array to hexadecimal string prior to .NET 5
                                                       // StringBuilder sb = new System.Text.StringBuilder();
                                                       // for (int i = 0; i < hashBytes.Length; i++)
                                                       // {
                                                       //     sb.Append(hashBytes[i].ToString("X2"));
                                                       // }
                                                       // return sb.ToString();
            }
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
