using LessonsBot_DB.ModelsDb;
using LessonsBot_Vk;
using LessonsBot_Vk.ExpDataset;
using LessonsBot_Vk.Libs;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static int _totalCount = 0;
    private static List<Bot> _bots;

    private static void Main(string[] args)
    {
        BootOn();

        SLogger.Write($"Всего подключенных ботов: {_totalCount}");

        /* Запуск ботов в потоке*/
        foreach (var item in _bots)
        {
            
            new Thread(() => new VkService(item)).Start();    
        }

        while (true)
        {
            string command = Console.ReadLine().ToLower();
            if (command == "add")
                AddBot();

            if (command == "train")
                new TrainBot().Start();
        }

    }



    private static void BootOn()
    {
        SLogger.Debug = true;
        SLogger.SaveLogs = true;

        CacheMigrator.Migrate();

        using (DbProvider _ef = new())
        {
            _totalCount = _ef.Bots.Count();
            _bots = _ef.Bots.Include(x => x.PeerProps).ToList();
        }
    }
    private static void AddBot()
    {
        Console.WriteLine("Введите токен:");
        string token = Console.ReadLine();

        Console.WriteLine("Введите ID бота:");
        long id = Convert.ToInt64(Console.ReadLine());

        Console.WriteLine("Введите задержку между запросами (мс):");
        int timeout = Convert.ToInt32(Console.ReadLine());

        Bot bot = new Bot()
        {
            Token = token,
            TimeOutResponce = timeout,
            IdValueService = id,
        };

        using(DbProvider _ef = new DbProvider())
        {
            _ef.Add(bot);
            _ef.SaveChanges();
        };

    }
}