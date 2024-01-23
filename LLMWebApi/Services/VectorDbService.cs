using LLMWebApi.Chatbot;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Microsoft.SemanticKernel.Memory;

namespace LLMWebApi.Services {
    public class VectorDbService : VectorDbClient {
        public static void Init(WebApplicationBuilder builder, Kernel kernel) 
        {
            AddQdrantConfiguration(builder);
            Init(kernel);
        }

#pragma warning disable SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        public static ISemanticTextMemory Memory {
#pragma warning restore SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            get {
                return memory!;
            }
        }

#pragma warning disable SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        public static QdrantMemoryStore MemoryStore {
#pragma warning restore SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            get {
                return memoryStore!;
            }
        }

        public static async Task GetCollections() 
        {
            IList<string> collections = await memory!.GetCollectionsAsync();

            Console.WriteLine($"No of collections: {collections.Count}");

            Console.WriteLine("--------Collections--------");

            foreach(var collection in collections) 
            {
                Console.WriteLine(collection);
            }
            Console.WriteLine("---------------------------");
        }

        public static void CreateEmbedding(string collection, string filePath) 
        {

        }
    }
}