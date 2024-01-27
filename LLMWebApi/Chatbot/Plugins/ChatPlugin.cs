using Microsoft.SemanticKernel;

namespace LLMWebApi.Chatbot.Plugins 
{
    public class ChatPlugin 
    {
        private Kernel kernel;
        private ILogger logger;

        public ChatPlugin(Kernel kernel, ILogger logger) 
        {
            this.kernel = kernel;
            this.logger = logger;
        }

        // private async Task<string> GetUserIntentAsync()
        // {
        //     var getIntentFunction = this.kernel.CreateFunctionFromPromptYaml();
        // }

        // private async Task<string> GetChatHistoryAsync()
        // {

        // }

        private async Task GetContext()
        {
            
        }
    }
}