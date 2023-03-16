using Microsoft.EntityFrameworkCore;

namespace LessonBot_SBot.Model
{
    public class DictionaryContext : DbContext
    {
        public DbSet<Dictionary> Dictionaries { get; set; }

        public DictionaryContext()
        {
          //  Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=dictionary.db");
        }
    }
}
