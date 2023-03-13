using LessonsBot_DB.ModelService;
using Microsoft.EntityFrameworkCore;

namespace LessonsBot_Vk.Libs
{
    internal class CacheMigrator
    {
        private static DbProvider _ef;
        public static void Migrate()
        {
            _ef = new DbProvider();
            _ef.Database.Migrate();

            SLogger.WriteWarning("Стартует процесс кеширования данных");

            try
            {
                var groups = ApiSgk.GetGroups();
                var teachers = ApiSgk.GetTeachers();

                if (_ef.GroupsCache.Count() == groups.Count && _ef.TeacherCaches.Count() == teachers.Count)
                    return;

                foreach (var item in groups)
                {
                    var find = _ef.GroupsCache.FirstOrDefault(x => x.Name == item.Name);

                    if (find != null)
                        continue;

                    SLogger.Write($"Обработка: #{item.Id} {item.Name}");
                    _ef.Add( new ApiGroups() { Id = item.Id, Name = item.Name });
                    _ef.SaveChanges();
                }

                foreach (var item in teachers)
                {

                    var find = _ef.TeacherCaches.FirstOrDefault(x => x.name == item.name);

                    if (find != null)
                        continue;

                    SLogger.Write($"Обработка: #{item.id} {item.name}");
                    _ef.Add(new ApiTeacher() { id = item.id, name = item.name });
                    _ef.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                SLogger.WriteDanger($"Failed to migrate data API:");
                SLogger.WriteDanger($"{ex.Message}");
            }
        }
    }
}
