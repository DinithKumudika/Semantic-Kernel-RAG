using System.ComponentModel;
using System.Runtime.CompilerServices;
using LLMWebApi.Chatbot;
using Microsoft.SemanticKernel;

namespace LLMWebApi.Services {
    public class BotService : Bot {
        public static void Init(WebApplicationBuilder builder) 
        {
            AddOpenAIConfiguration(builder);
            CreateBasicKernelBuilder();
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