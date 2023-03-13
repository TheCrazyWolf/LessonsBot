using LessonsBot_DB.ModelService;

namespace LessonsBot_Vk.Libs
{
    internal class CacheMigrator
    {
        private static DbProvider _ef;
        public static void Migrate()
        {
            _ef = new DbProvider();

            SLogger.WriteWarning("Стартует процесс кеширования данных");
            try
            {
                var groups = ApiSgk.GetGroups();
                var teachers = ApiSgk.GetTeachers();

                if (_ef.GroupsCache.Count() == groups.Count)
                    return;
                if (_ef.TeacherCaches.Count() == teachers.Count)
                    return;

                foreach (var item in groups)
                {

                    var find = _ef.GroupsCache.FirstOrDefault(x => x.Name == item.Name);

                    if (find != null)
                        continue;

                    SLogger.Write($"Обработка: #{item.Id} {item.Name}");
                    _ef.Add( new ApiGroupsCache() { Id = item.Id, Name = item.Name });
                    _ef.SaveChanges();
                }

                foreach (var item in teachers)
                {

                    var find = _ef.TeacherCaches.FirstOrDefault(x => x.name == item.name);

                    if (find != null)
                        continue;

                    SLogger.Write($"Обработка: #{item.id} {item.name}");
                    _ef.Add(new ApiTeacherCache() { id = item.id, name = item.name });
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
