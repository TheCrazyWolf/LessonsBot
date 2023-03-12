using LessonsBot_DB.ModelService;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

public class ApiSgk
{
    public static ApiLessons GetLessons(TypeLesson type, 
        DateOnly date, string id)
    {
        string json = "";

        switch (type)
        {
            case TypeLesson.Group:
                json = Response($"https://asu.samgk.ru/api/schedule/{id}/{date.ToString("yyyy - MM - dd")}");
                break;
            case TypeLesson.Teacher:
                json = Response($"https://asu.samgk.ru/api/schedule/teacher/{date.ToString("yyyy-MM-dd")}/{id}");
                break;
            case TypeLesson.Cabinet:
                json = Response($"https://asu.samgk.ru/api/schedule/cabs/{date.ToString("yyyy-MM-dd")}/cabNum/{id}");
                break;
        }

        return JsonConvert.DeserializeObject<ApiLessons>(json);
    }

    public static List<ApiTeacher> GetTeachers()
        => JsonConvert.DeserializeObject<List<ApiTeacher>>(Response("https://asu.samgk.ru/api/teachers"));
    public static List<ApiGroups> GetGroups()
        => JsonConvert.DeserializeObject<List<ApiGroups>>(Response("https://mfc.samgk.ru/api/groups"));

    static string Response(string url)
    {
        try
        {
            SLogger.Write($"Отправляется запрос к {url}");
            using (var wb = new WebClient())
            {
                wb.Headers.Set("Accept", "application/json");
                wb.Headers.Set("origin", "http://samgk.ru");
                wb.Headers.Set("Referer", "samgk.ru");
                return wb.DownloadString(url);
            }
        }
        catch (Exception ex)
        {
            SLogger.WriteDanger(ex.ToString());
        }

        return null;
    }
}