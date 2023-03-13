
public class SLogger
{

    /* Логирование да? нет?*/

    /* Пишем в файл Да, нет*/

    public static bool Debug = true;
    public static bool SaveLogs = true;

    protected static string filename = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.txt";

    public static void Write(object data)
    {

        if (!Debug)
            return;

        DateTime timeset = DateTime.Now;

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"[{timeset}] > {data}");
        Console.ForegroundColor = ConsoleColor.White;

        if (!SaveLogs)
            return;

        LoggToFile(timeset, data);

    }

    public static void WriteWarning(object data)
    {

        if (!Debug)
            return;

        DateTime timeset = DateTime.Now;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[{timeset}] > {data}");
        Console.ForegroundColor = ConsoleColor.White;

        if (!SaveLogs)
            return;

        LoggToFile(timeset, data);

    }

    public static void WriteDanger(object data)
    {

        if (!Debug)
            return;

        DateTime timeset = DateTime.Now;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{timeset}] > {data}");
        Console.ForegroundColor = ConsoleColor.White;

        if (!SaveLogs)
            return;

        LoggToFile(timeset, data);

    }

    public static void LoggToFile(DateTime timeset, object data)
    {
       
        Directory.CreateDirectory("logcat");

        if (!File.Exists($"{Environment.CurrentDirectory}\\logcat\\" + filename))
            File.WriteAllText($"logcat\\" + filename, "");

        var logcat = File.ReadAllText($"{Environment.CurrentDirectory}\\logcat\\" + filename);

        logcat += $"\n[{timeset}] {data}";

        File.WriteAllText($"{Environment.CurrentDirectory}\\logcat\\" + filename, logcat);
    }
}