﻿using LessonsBot_DB.ModelsDb;
using LessonsBot_DB.ModelService;
using LessonsBot_Vk;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    static DbProvider _ef;
    private static void Main(string[] args)
    {
        _ef = new DbProvider();
        _ef.Database.Migrate();

        SLogger.Write($"Всего подключенных ботов: {_ef.Bots.Count()}");

        foreach (var item in _ef.Bots.Include(x=> x.PeerProps))
        {
            new Thread( () => new VkService(item, ref _ef).Start() ).Start();
        }

        while (true)
        {
            string command = Console.ReadLine().ToLower();
            if (command == "add")
                AddBot();
        }

        //var s = ApiSgk.GetLessons(TypeLesson.Teacher, new DateOnly(2023, 03, 13), 1468.ToString());
        //SLogger.Write(s);

        //foreach (dynamic item in s.lessons)
        //{
        //    SLogger.WriteDanger(item.title);
        //}
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

        _ef.Add(bot);
        _ef.SaveChanges();
    }
}