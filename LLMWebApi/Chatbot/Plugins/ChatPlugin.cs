using System.ComponentModel;
using LLMWebApi.Chatbot.Prompts;
using LLMWebApi.Services;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

namespace LLMWebApi.Chatbot.Plugins
{
    /// <summary>
    /// ChatPlugin offers chat experience by using memories.
    /// Also used to extract conversation history and user intentions.
    /// </summary>
    public sealed class ChatPlugin 
    {
        /// <summary>
        /// Extract user intention from the request
        /// </summary>
        /// <returns>
        /// user intent (string)
        /// </returns>

        [KernelFunction, Description("Extract the user's intention from the user query")]
        public static async Task<string> ExtractUserIntent([Description("user's query to the chatbot")] string query)
        {
            ChatHistory chatHistory = [];
            
            var ExtractIntentPromptYaml = Path.Combine(Directory.GetCurrentDirectory(), "Chatbot", "Prompts", "ExtractIntent", "ExtractIntent.yaml");
            
            var ExtractIntentPrompt = File.ReadAllText(ExtractIntentPromptYaml);

            var extractIntentFunction = BotService.BotKernel.CreateFunctionFromPromptYaml(
                ExtractIntentPrompt,
                promptTemplateFactory: new HandlebarsPromptTemplateFactory()
            );

            var handlebarArguments = new KernelArguments ()
            {
                {"query", query},
                {"choices", PromptsOptions.extractIntentChoices},
                {"fewShotExamples", PromptsOptions.extractIntentFewShot},
                {"history", chatHistory}
            };

            var intent = await BotService.BotKernel.InvokeAsync(
                function: extractIntentFunction,
                arguments: handlebarArguments
            );

            return intent.ToString();
        }

        // private async Task<string> GetChatHistoryAsync()
        // {

        // }
    }
}