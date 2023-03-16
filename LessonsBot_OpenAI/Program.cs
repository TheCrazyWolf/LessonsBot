using LessonsBot_OpenAI;
using OpenAI;
using OpenAI.Models.ChatCompletion;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string _token = "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";

        OpenAiClient _client = new OpenAiClient(_token);

        while (true)
        {
            string enter = Console.ReadLine();
            string text = await _client.GetChatCompletions(new UserMessage(enter), maxTokens: 80);
            Console.WriteLine($"AI: {text}");
        }
    }
}