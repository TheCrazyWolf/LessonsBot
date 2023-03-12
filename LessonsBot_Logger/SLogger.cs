
public class SLogger
{
    public static bool Debug = true;
    public static bool SaveLogs = true;

    public static void Write(object data)
    {

        if (!Debug)
            return;

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"[{DateTime.Now}] > {data}");
        Console.ForegroundColor = ConsoleColor.White;

        if (!SaveLogs)
            return;

        LoggToFile(data);

    }

    public static void WriteWarning(object data)
    {

        if (!Debug)
            return;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[{DateTime.Now}] > {data}");
        Console.ForegroundColor = ConsoleColor.White;

        if (!SaveLogs)
            return;

        LoggToFile(data);

    }

    public static void WriteDanger(object data)
    {

        if (!Debug)
            return;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{DateTime.Now}] > {data}");
        Console.ForegroundColor = ConsoleColor.White;

        if (!SaveLogs)
            return;

        LoggToFile(data);

    }

    public static void LoggToFile(object data)
    {
        //if(File.)
    }
}