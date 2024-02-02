using System.Text.Json;
using Json.More;

namespace LLMWebApi.Chatbot.Helpers {
    public static class TokenUsage 
    {
        public static void GetTokenCost(IReadOnlyDictionary<string, object?> metadata) 
        {
            var openAIUsage = metadata["Usage"];
            JsonDocument usageJson = openAIUsage.ToJsonDocument();
            Console.WriteLine("----OpenAI Usage----");
            Console.WriteLine($"Completion Tokens: {usageJson.RootElement.GetProperty("CompletionTokens")}");
            Console.WriteLine($"Prompt Tokens: {usageJson.RootElement.GetProperty("PromptTokens")}");
            Console.WriteLine($"Total Tokens: {usageJson.RootElement.GetProperty("TotalTokens")}");
            Console.WriteLine("--------------------");
        }
        
    }
}