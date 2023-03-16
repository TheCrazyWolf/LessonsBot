using System.ComponentModel.DataAnnotations;

namespace LessonBot_SBot.Model
{
    public class Dictionary
    {
        [Key]
        public long IdWord { get; set; }
        public string Word { get; set; }
        public string Answer { get; set; }
    }
}
