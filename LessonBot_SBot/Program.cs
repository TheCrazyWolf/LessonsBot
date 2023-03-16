using LessonBot_SBot;

internal class Program
{
    private static void Main(string[] args)
    {

        Task.Run( () => new SimpleBot().Train("database1.txt"));
        Task.Run( () => new SimpleBot().Train("database2.txt"));

        while (true)
        {
            var answer = new SimpleBot().GetResult(Console.ReadLine());
            Console.WriteLine($"BOT: {answer}");
        }
    }
}