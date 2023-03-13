
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LessonsBot_DB.ModelService
{
    [Serializable]
    public class ApiTeacher
    {
        public string id { get; set; }

        private string _name;
        public string name
        {
            get => _name;
            set
            {
                _name = Regex.Replace(value, @"\s+", " ");
            }
        }
    }


}
