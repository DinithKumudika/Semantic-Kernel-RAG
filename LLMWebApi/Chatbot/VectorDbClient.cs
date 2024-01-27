using System.Runtime.CompilerServices;
using LLMWebApi.Exceptions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Microsoft.SemanticKernel.Memory;
using static LLMWebApi.Configuration.Configuration;

namespace LLMWebApi.Chatbot
{
    public class VectorDbClient
    {
        protected static QdrantConfig? qdrantConfig;
        protected static OpenAIConfig? openAIConfig;
#pragma warning disable SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        protected static ISemanticTextMemory? memory;
        protected static QdrantMemoryStore? memoryStore;
#pragma warning restore SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        protected static void AddQdrantConfiguration(WebApplicationBuilder builder) 
        {
            // Get Qdrant configuration from user secrets
            qdrantConfig = builder.Configuration.GetSection("Qdrant").Get<QdrantConfig>() ?? throw new ConfigurationNotFoundException();
            openAIConfig = builder.Configuration.GetSection("OpenAI").Get<OpenAIConfig>() ?? throw new ConfigurationNotFoundException();

            if(qdrantConfig.Endpoint!.Length > 0 && qdrantConfig.Port!.Length > 0)
            {
                Console.WriteLine("Qdrant Configuration Completed...");
            }
            else 
            {
                Console.WriteLine("Qdrant Configuration Failed...");
            }
        }

#pragma warning disable SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        protected static void Init(Kernel kernel) {
#pragma warning disable SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            memoryStore = new QdrantMemoryStore(
                endpoint: qdrantConfig!.Endpoint!, 
                vectorSize: 1536
            );

            memory = new MemoryBuilder().WithMemoryStore(memoryStore)
            .WithLoggerFactory(kernel.LoggerFactory)
            .WithTextEmbeddingGeneration(
                new OpenAITextEmbeddingGenerationService(
                    openAIConfig!.EmbeddingModelId!,
                    openAIConfig!.ApiKey!,
                    openAIConfig.OrgId
                    )
                ).Build();
        }
#pragma warning restore SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    }
}
