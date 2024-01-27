using HandlebarsDotNet.Helpers.Enums;
using LLMWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LLMWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MemoryController : ControllerBase
    {
        private readonly ILogger<MemoryController> _logger;
        private readonly EmbeddingService embeddingService;

        public MemoryController (ILogger<MemoryController> logger)
        {
            _logger = logger;
            this.embeddingService = new EmbeddingService();
        }

        // Create embeddings in a collection
        [HttpGet("create/{category}")]
        public async Task<IResult> CreateMemory(string category)
        {
            Console.WriteLine($"adding memories to {category} collection");
            string dataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data");

            var docFolder = Path.Combine(dataFilePath, category);
            var documentService = new DocumentService(docFolder);

            var embeddings = await embeddingService.BuildEmbeddings(category, documentService);

            return Results.Ok(embeddings);
        }

        [HttpDelete("{collection}")]
        public async Task<IResult> DeleteMemory(string collection, [FromQuery(Name ="id")] string memoryUid) 
        {
            await embeddingService.RemoveEmbeddings(collection, memoryUid);
            return Results.Ok($"embedding with {memoryUid} deleted successfully!");
        }
    }
}