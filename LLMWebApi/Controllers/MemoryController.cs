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
        // http://localhost:5031/api/memory/create/terms-and-conditions
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

        // Delete a memory from a collection
        [HttpDelete("{collection}")]
        public async Task<IResult> DeleteMemory(string collection, [FromQuery(Name ="uid")] string memoryUid) 
        {
            var result = await embeddingService.RemoveEmbeddings(collection, memoryUid);
            return Results.Ok(result);
        }

        // Delete set of memories from a collection
        [HttpDelete("batch/{collection}")]
        public async Task<IResult> DeleteMemoryBatch(string collection, [FromQuery(Name ="uids")] string[] memoryUids) 
        {
            var result = await embeddingService.RemoveEmbeddingsBatch(collection, memoryUids);
            return Results.Ok(result);
        }
    }
}