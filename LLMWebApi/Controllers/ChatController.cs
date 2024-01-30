using HandlebarsDotNet;
using LLMWebApi.Chatbot.Plugins;
using LLMWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

namespace LLMWebApi.Controllers {

    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase 
    {
        private readonly ILogger<ChatController> _logger;
        private readonly ChatPlugin chatPlugin;
        
        public ChatController(ILogger<ChatController> logger)
        {
            this._logger = logger;
        }
        
        [HttpPost("intent")]
        public async Task<IResult> GetIntent([FromBody] dynamic data)
        {
            string query = data.GetProperty("query").ToString();

            var arguments = new KernelArguments ()
            {
                {"query", query},
            };

            KernelPluginCollection kernelPlugins = BotService.BotKernel.Plugins;

            Console.WriteLine($"Plugins registered in the kernel : {kernelPlugins.Count}");

            foreach(var plugin in kernelPlugins)
            {
                Console.WriteLine("<-------- Plugin List -------->");
                Console.WriteLine(plugin.Name);
                Console.WriteLine(plugin.Description);
                Console.WriteLine("-------------------------------");
            }
            
            var intent = await BotService.BotKernel.InvokeAsync(
                "ChatPlugin", 
                "ExtractUserIntent",
                arguments
            );

            return Results.Ok(intent.ToString());
        }
    }
}