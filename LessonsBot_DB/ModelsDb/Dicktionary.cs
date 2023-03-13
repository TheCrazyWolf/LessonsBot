using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonsBot_DB.ModelsDb
{
    public class Dicktionary
    {
        [Key]
        public Guid IdDicktionary { get; set; }
        public string Word { get; set; }
        public string Answer { get; set; }
    }
}
