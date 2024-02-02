using System.ComponentModel;
using LLMWebApi.Chatbot.Prompts;
using LLMWebApi.Services;
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
            chatHistory.Add(PromptsOptions.masterPrompt);
            
            var ExtractIntentPromptYaml = Path.Combine(
                Directory.GetCurrentDirectory(), 
                "Chatbot", 
                "Prompts", 
                "ExtractIntent", 
                "ExtractIntent.yaml"
            );
            
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

        [KernelFunction, Description("Find the Qdrant db collection that match with a user intent")]
        public async Task<string> MapIntentToCollection([Description("intent extracted from a user query")] string intent)
        {
            ChatHistory chatHistory = [];
            var MapIntentToCollectionPromptYaml = Path.Combine(
                Directory.GetCurrentDirectory(), 
                "Chatbot", 
                "Prompts", 
                "MapIntentToCollection", 
                "MapIntentToCollection.yaml"
            );

            var ExtractIntentPrompt = File.ReadAllText(MapIntentToCollectionPromptYaml);

            var mapIntentToCollectionFunction = BotService.BotKernel.CreateFunctionFromPromptYaml(
                ExtractIntentPrompt,
                promptTemplateFactory: new HandlebarsPromptTemplateFactory()
            );

            var handlebarArguments = new KernelArguments ()
            {
                {"query", intent},
                {"history", chatHistory},
                {"collections", await VectorDbService.GetCollections()}
            };

            var collection = await BotService.BotKernel.InvokeAsync(
                function: mapIntentToCollectionFunction,
                arguments: handlebarArguments
            );


            return collection.ToString();
        }

        // private async Task<string> GetChatHistoryAsync()
        // {

        // }
    }
}