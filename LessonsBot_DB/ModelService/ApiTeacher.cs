
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LessonsBot_DB.ModelService
{
    [Serializable]
    public class ApiTeacher
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}
