using OpenAI;
using OpenAI.Models.ChatCompletion;

namespace LessonsBot_OpenAI
{
    public static class ApiOpenAI
    {
        private static readonly string _token = "";
        private static OpenAiClient _client = new OpenAiClient(_token);

        public static async Task<string> GetResponce(string message)
            => await _client.GetChatCompletions(new UserMessage(message), maxTokens: 150);
    }
}
