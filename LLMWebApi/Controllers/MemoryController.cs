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
        public async Task<ActionResult> CreateMemoriesFromFile([FromBody] Document document)
        {
            
            if(document == null)
            {
                return BadRequest(document);
            }

            bool collectionExists = await VectorDbService.HasCollection(document.Collection!);

            if(!collectionExists)
            {
                Console.WriteLine($"{document.Collection} collection doesn't exists");

                if(await VectorDbService.CreateCollection(document.Collection!))
                {
                    Console.WriteLine($"{document.Collection} collection created...");
                }
                else 
                {
                    Console.WriteLine($"Couldn't create {document.Collection}. Make sure Qdrant service is running...");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            
            Console.WriteLine($"adding memories to {document.Collection} collection");
            var documentService = new DocumentService(document);
            var embeddings = await embeddingService.BuildEmbeddingsFromFile(documentService);

            if(embeddings.Count == 0)  
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status201Created, embeddings);
        }


        [HttpGet("{id}", Name ="GetMemory")]
        public IResult GetMemory(string id) 
        {
            return Results.Ok();
        }

        // Delete a memory from a collection
        [HttpDelete("{collection}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMemory(string collection, string id) 
        {
            var result = await embeddingService.RemoveEmbeddings(collection, id);
            
            if(result.Status == StatusCodes.Status200OK) 
            {
                return Ok(result);
            }
            else if (result.Status == StatusCodes.Status500InternalServerError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            else 
            {
                return NotFound(result);
            }
        }

        // Delete set of memories from a collection
        [HttpDelete("batch/{collection}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> DeleteMemoryBatch(string collection, [FromQuery(Name ="uids")] string[] memoryUids) 
        {
            var result = await embeddingService.RemoveEmbeddingsBatch(collection, memoryUids);
            return Results.Ok(result);
        }
    }
}