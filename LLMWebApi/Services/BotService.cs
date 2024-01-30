using LLMWebApi.Chatbot;
using LLMWebApi.Chatbot.Plugins;
using Microsoft.SemanticKernel;

namespace LLMWebApi.Services {
    public class BotService : Bot {
        public static void Init(WebApplicationBuilder builder) 
        {
            AddOpenAIConfiguration(builder);
            CreateBasicKernelBuilder();
            // Register plugins with the kernel
            var pluginDir = Path.Combine(Directory.GetCurrentDirectory(), "Chatbot", "Plugins");
            Console.WriteLine($"Plugins Directory: {pluginDir}");
            kernelBuilder!.Plugins.AddFromType<ChatPlugin>();
            BuildKernel();
        }
        public static IKernelBuilder KernelBuilder {
            get {
                return kernelBuilder!;
            }
        }

        public static Kernel BotKernel {
            get {
                return botKernel!;
            }
        }
    }
}