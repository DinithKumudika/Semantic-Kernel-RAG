using System.Text.Json;
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
        [HttpPost("create/{collection}")]
        public async Task<IResult> CreateMemory(string collection, [FromBody] dynamic data)
        {
            Console.WriteLine($"adding memories to {collection} collection");
            string dataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data");

            var docFolder = Path.Combine(dataFilePath, data.GetProperty("dataDir").ToString());
            var documentService = new DocumentService(docFolder);

            var embeddings = await embeddingService.BuildEmbeddings(collection, documentService);

            return Results.Ok(embeddings);
        }

        // Delete a memory from a collection
        [HttpDelete("{collection}/{uid}")]
        public async Task<IResult> DeleteMemory(string collection, string uid) 
        {
            var result = await embeddingService.RemoveEmbeddings(collection, uid);
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