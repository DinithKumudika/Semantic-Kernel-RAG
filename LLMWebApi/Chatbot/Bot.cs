using LLMWebApi.Exceptions;
using Microsoft.SemanticKernel;
using static LLMWebApi.Configuration.Configuration;

namespace LLMWebApi.Chatbot
{
    public class Bot
    {
        public static IConfigurationRoot? Configuration { get; set; }

        protected static IKernelBuilder? kernelBuilder;
        protected static Kernel? botKernel;
        protected static OpenAIConfig? openAIConfig;
        protected static QdrantConfig? qdrantConfig;

        protected static void AddOpenAIConfiguration(WebApplicationBuilder builder)
        {
            // Get Open AI configuration from user secrets

            openAIConfig = builder.Configuration.GetSection("OpenAI").Get<OpenAIConfig>() ?? throw new ConfigurationNotFoundException();

            if (openAIConfig.ModelId.Length > 0 && openAIConfig.ApiKey.Length > 0 && openAIConfig.OrgId.Length > 0)
            {
                Console.WriteLine($"using model {openAIConfig.ModelId}");
                Console.WriteLine("Open AI Configuration Completed...");
            }
            else
            {
                Console.WriteLine("Open AI Configuration Failed...");
            }
        }

        protected static void AddQdrantConfiguration(WebApplicationBuilder builder) 
        {
            // Get Qdrant configuration from user secrets
        }

        protected static void BuildKernel() 
        {
            botKernel = kernelBuilder!.Build();
        }

        protected static void CreateBasicKernelBuilder()
        {
            kernelBuilder = Kernel.CreateBuilder();

            // Add Console logger and Debug logger
            kernelBuilder.Services.AddLogging(c => c
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole()
                .AddDebug()
            );

            kernelBuilder.AddOpenAIChatCompletion(
                openAIConfig.ModelId,
                openAIConfig.ApiKey,
                openAIConfig.OrgId
            );

            if(kernelBuilder != null) 
            {
                Console.WriteLine("Basic Kernel Configuration Completed...");
            }
        }

        private static IKernelBuilder AddNativePlugins(IKernelBuilder kernelBuilder, List<IKernelBuilderPlugins> plugins)
        {
            return kernelBuilder;
        }

        private static IKernelBuilder AddSemanticPlugins(IKernelBuilder kernelBuilder, List<IKernelBuilderPlugins> plugins)
        {
            return kernelBuilder;
        }

    }
}