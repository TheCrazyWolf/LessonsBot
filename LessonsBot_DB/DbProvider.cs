using LessonsBot_DB.ModelsDb;
using LessonsBot_DB.ModelService;
using Microsoft.EntityFrameworkCore;

public class DbProvider : DbContext
{
    public DbSet<Bot> Bots { get; set; }
    public DbSet<PeerProp> PeerProps { get; set; }
    public DbSet<ApiGroups> GroupsCache { get; set; }
    public DbSet<ApiTeacher> TeacherCaches { get; set; }

    private string _path = "data.db";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_path}");
    }

    private static void Main(string[] args)
    {
    }
}