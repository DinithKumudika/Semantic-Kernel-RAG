using LLMWebApi.Exceptions;
using Microsoft.SemanticKernel;
using static LLMWebApi.Configuration.Configuration;

namespace LLMWebApi.Chatbot
{
    public class Bot
    {
        protected static IKernelBuilder? kernelBuilder;
        protected static Kernel? botKernel;
        protected static OpenAIConfig? openAIConfig;

        protected static void AddOpenAIConfiguration(WebApplicationBuilder builder)
        {
            // Get Open AI configuration from user secrets
            openAIConfig = builder.Configuration.GetSection("OpenAI").Get<OpenAIConfig>() ?? throw new ConfigurationNotFoundException();

            if (openAIConfig!.ChatModelId!.Length > 0 && openAIConfig!.EmbeddingModelId!.Length > 0 && openAIConfig!.ApiKey!.Length > 0 && openAIConfig!.OrgId!.Length > 0)
            {
                Console.WriteLine($"chat model : {openAIConfig.ChatModelId}");
                Console.WriteLine($"embedding model : {openAIConfig.EmbeddingModelId}");
                Console.WriteLine("Open AI Configuration Completed...");
            }
            else
            {
                Console.WriteLine("Open AI Configuration Failed...");
            }
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

#pragma warning disable SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            kernelBuilder
            .AddOpenAIChatCompletion(
                openAIConfig!.ChatModelId!,
                openAIConfig!.ApiKey!,
                openAIConfig.OrgId
            )
            .AddOpenAITextEmbeddingGeneration(
                openAIConfig!.EmbeddingModelId!,
                openAIConfig!.ApiKey!,
                openAIConfig.OrgId
            );
#pragma warning restore SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            if (kernelBuilder != null)
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