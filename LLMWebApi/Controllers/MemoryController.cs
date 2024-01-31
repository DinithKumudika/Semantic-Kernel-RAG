using LLMWebApi.Models;
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

        // Create embeddings in a collection using document
        // http://localhost:5031/api/memory/create
        [HttpPost("create", Name="CreateMemoriesFromFile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IResult> CreateMemoriesFromFile([FromBody] Document document)
        {
            
            if(document == null)
            {
                return (IResult)BadRequest(document);
            }
            
            Console.WriteLine($"adding memories to {document.Collection} collection");
            var documentService = new DocumentService(document);
            var embeddings = await embeddingService.BuildEmbeddingsFromFile(documentService);

            if(embeddings.Count == 0)  
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Results.CreatedAtRoute("GetMemory",embeddings);
        }


        [HttpGet("{id}", Name ="GetMemory")]
        public IResult GetMemory(string id) 
        {
            return Results.Ok();
        }

        // Delete a memory from a collection
        [HttpDelete("{collection}/{uid}")]
        [ProducesResponseType(200)]
        public async Task<IResult> DeleteMemory(string collection, string uid) 
        {
            var result = await embeddingService.RemoveEmbeddings(collection, uid);
            return Results.Ok(result);
        }

        // Delete set of memories from a collection
        [HttpDelete("batch/{collection}")]
        [ProducesResponseType(200)]
        public async Task<IResult> DeleteMemoryBatch(string collection, [FromQuery(Name ="uids")] string[] memoryUids) 
        {
            var result = await embeddingService.RemoveEmbeddingsBatch(collection, memoryUids);
            return Results.Ok(result);
        }
    }
}