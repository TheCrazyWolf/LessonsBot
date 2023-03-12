
namespace LessonsBot_DB.ModelService
{
    [Serializable]
    public class Lesson
    {
        public string title { get; set; }
        public string num { get; set; }
        public string teachername { get; set; }
        public object nameGroup { get; set; }
        public string cab { get; set; }
        public string resource { get; set; }
    }
}
