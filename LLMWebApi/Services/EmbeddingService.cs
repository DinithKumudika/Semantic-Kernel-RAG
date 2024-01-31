using LLMWebApi.Exceptions;
using LLMWebApi.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;
using static LLMWebApi.Configuration.Configuration;

namespace LLMWebApi.Services
{
    public class EmbeddingService
    {
        protected static IKernelBuilder? kernelBuilder;
        protected static OpenAIConfig? openAIConfig;

        // Register embedding service with the kernel
        public static void Init(IKernelBuilder kernelBuilder, WebApplicationBuilder builder)
        {
            openAIConfig = builder.Configuration.GetSection("OpenAI").Get<OpenAIConfig>() ?? throw new ConfigurationNotFoundException();

#pragma warning disable SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            kernelBuilder.AddOpenAITextEmbeddingGeneration(
                openAIConfig!.EmbeddingModelId!,
                openAIConfig!.ApiKey!,
                openAIConfig.OrgId
            );
#pragma warning restore SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }

        // create Ids for each embedding
        public static string GetEmbeddingId(string document, int pageNo, int paragraphIdx)
        {
            string docName = document.Split(".")[0];
            return $"{docName}-{pageNo}-{paragraphIdx + 1}";
        }

        public async Task<List<Memory>> BuildEmbeddingsFromFile(DocumentService documentService)
        {
            if (documentService.document != null)
            {
                List<Memory> embeddings = [];
                var documentText = documentService.ExtractFromDocument();

                if (documentText != null)
                {

                    Console.WriteLine($"creating embeddings for {documentService.document.FileName}...");

                    foreach (var pageContent in documentText.Content)
                    {
                        Console.WriteLine($"creating embeddings for paragraphs in page {pageContent.PageNo}...");
                        var paragraphs = pageContent.Paragraphs;

                        foreach (var paragraph in pageContent.Paragraphs)
                        {
                            Console.WriteLine($"creating embedding for paragraph {pageContent.Paragraphs.IndexOf(paragraph) + 1}...");

                            string id = GetEmbeddingId(
                                documentService.document.FileName,
                                pageContent.PageNo,
                                pageContent.Paragraphs.IndexOf(paragraph)
                            );

                            string uid = await VectorDbService.Memory.SaveInformationAsync(
                                documentService.document.Collection!,
                                paragraph,
                                id
                            );

                            if (uid != null)
                            {
                                var createdAt = DateTime.Now.ToLocalTime().ToString();

                                Console.WriteLine($"id of saved memory record: {id}");
                                Console.WriteLine($"unique id of saved memory record: {uid}");

                                var memory = new Memory
                                {
                                    UId = uid,
                                    Id = id,
                                    Collection = documentService.document.Collection!,
                                    Description = documentService.document.Description,
                                    Metadata = documentService.document.Metadata,
                                    CreatedAt = createdAt
                                };

                                embeddings.Add(memory);
                            }
                            else
                            {
                                return [];
                            }
                        }
                    }
                    return embeddings;
                }
                return [];
            }

            return [];

        }

        public async Task<string> RemoveEmbeddings(string collection, string uid)
        {
            await VectorDbService.Memory.RemoveAsync(collection, uid);

#pragma warning disable SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            MemoryQueryResult? result = await VectorDbService.Memory.GetAsync(collection, uid);

#pragma warning restore SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            if (result == null)
            {
                return $"Memory with uid {uid} deleted successfully";
            }

            return $"Error deleting memory with {uid}";
        }

        public async Task<string> RemoveEmbeddingsBatch(string collection, string[] uids)
        {
            var NoOfmemoriesToDelete = uids.Length;
            List<string> retrievedMemories = [];
            List<string> nonDeletedUids = [];

#pragma warning disable SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            await VectorDbService.MemoryStore.RemoveBatchAsync(collection, uids);
#pragma warning restore SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

#pragma warning disable SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            await foreach (var memory in VectorDbService.MemoryStore.GetBatchAsync(collection, uids))
            {
                retrievedMemories.Add(memory.Key);
            }
#pragma warning restore SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            if (retrievedMemories.Count != NoOfmemoriesToDelete)
            {
                foreach (var uid in uids)
                {
                    if (retrievedMemories.Contains(uid))
                    {
                        continue;
                    }
                    nonDeletedUids.Add(uid);
                }

                return $"Error deleting memories with uid {nonDeletedUids}";
            }

            return $"All {NoOfmemoriesToDelete} memories deleted successfully!";
        }
    }
}