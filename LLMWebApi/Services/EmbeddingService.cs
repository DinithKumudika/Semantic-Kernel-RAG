using LLMWebApi.Exceptions;
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

#pragma warning disable SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        public async Task<List<Dictionary<string, string>>> BuildEmbeddings(string collection, DocumentService documentService)
#pragma warning restore SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        {
            List<Dictionary<string, object>> documents = documentService.ExtractFromDocumentDir();
            List<Dictionary<string, string>> embeddings = [];

            if(documents.Count != 0)
            {
                foreach (var doc in documents)
                {
                    string documentName = (string)doc["pdfName"];
                    Console.WriteLine($"creating embeddings for {documentName}...");

                    List<Dictionary<string, object>> pageContents = (List<Dictionary<string, object>>)doc["content"];
                    
                    foreach (var pageContent in pageContents)
                    {
                        int pageNo = (int)pageContent["pageNo"];
                        Console.WriteLine($"creating embeddings for paragraphs in page {pageNo}...");
                        List<string> paragraphs = (List<string>)pageContent["paragraphs"];

                        foreach (var paragraph in paragraphs)
                        {
                            Console.WriteLine($"creating embedding for paragraph {paragraphs.IndexOf(paragraph) + 1}...");

                            string id = GetEmbeddingId(documentName, pageNo, paragraphs.IndexOf(paragraph));
                            string uid = await VectorDbService.Memory.SaveInformationAsync(collection, paragraph, id);

                            Console.WriteLine($"id of saved memory record: {id}");
                            Console.WriteLine($"unique id of saved memory record: {uid}");

                            Dictionary<string, string> point = [];
                            point.Add("uid", uid);
                            point.Add("id", id);
                            
                            embeddings.Add(point);
                        } 
                    }
                }
            }
            return embeddings;
        }

        public async Task RemoveEmbeddings(string collection, string uid)
        {
            await VectorDbService.Memory.RemoveAsync(collection, uid);
        }
    }
}