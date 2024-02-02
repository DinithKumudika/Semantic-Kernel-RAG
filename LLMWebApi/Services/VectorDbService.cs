using LLMWebApi.Chatbot;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Microsoft.SemanticKernel.Memory;

namespace LLMWebApi.Services
{
    public class VectorDbService : VectorDbClient
    {
        public static void Init(WebApplicationBuilder builder, Kernel kernel)
        {
            AddQdrantConfiguration(builder);
            Init(kernel);
        }

#pragma warning disable SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        public static ISemanticTextMemory Memory
        {
#pragma warning restore SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            get
            {
                return memory!;
            }
        }

#pragma warning disable SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        public static QdrantMemoryStore MemoryStore
        {
#pragma warning restore SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            get
            {
                return memoryStore!;
            }
        }

        public static async Task<IList<string>?> GetCollections()
        {
            IList<string> collections = await memory!.GetCollectionsAsync();

            if (collections.Count != 0)
            {
                Console.WriteLine($"No of collections: {collections.Count}");

                Console.WriteLine("--------Collections--------");


                foreach (var collection in collections)
                {
                    Console.WriteLine(collection);
                }
                Console.WriteLine("---------------------------");

                return collections;
            }

            return null;
        }

        public static async Task<bool> HasCollection(string collection)
        {
            IList<string> collections = await memory!.GetCollectionsAsync();

            if (collections.Contains(collection))
            {
                return true;
            }

            return false;

        }

        public static async Task<bool> CreateCollection(string collection)
        {
#pragma warning disable SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            await memoryStore!
            .CreateCollectionAsync(collection);
#pragma warning restore SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            var isCollectionExists = await HasCollection(collection);

            if (isCollectionExists)
            {
                return true;
            }
            return false;
        }

        public static async Task<bool> IsMemoryExists(string collection, string id)
        {
#pragma warning disable SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            MemoryQueryResult? result = await VectorDbService.Memory.GetAsync(collection, id);
#pragma warning restore SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            if (result == null)
            {
                return false;
            }

            return true;
        }
    }
}