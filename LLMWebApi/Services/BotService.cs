using System.ComponentModel;
using System.Runtime.CompilerServices;
using LLMWebApi.Chatbot;
using Microsoft.SemanticKernel;

namespace LLMWebApi.Services {
    public class BotService : Bot {
        public static void Init(WebApplicationBuilder builder) 
        {
            Bot.AddOpenAIConfiguration(builder);
            Bot.CreateBasicKernelBuilder();
            Bot.BuildKernel();
        }
        public static IKernelBuilder KernelBuilder {
            get {
                return Bot.kernelBuilder!;
            }
        }

        public static Kernel BotKernel {
            get {
                return Bot.botKernel!;
            }
        }
    }
}