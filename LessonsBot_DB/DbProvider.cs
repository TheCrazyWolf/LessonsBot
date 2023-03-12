using LessonsBot_DB.ModelsDb;
using Microsoft.EntityFrameworkCore;

public class DbProvider : DbContext
{
    public DbSet<Bot> Bots { get; set; }
    public DbSet<PeerProp> PeerProps { get; set; }

    private string _path = "data.db";

    public DbProvider()
    {

    }

    public DbProvider(string another_path)
    {
        _path = another_path;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_path}");
    }

    private static void Main(string[] args)
    {
    }
}