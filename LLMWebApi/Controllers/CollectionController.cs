using LLMWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LLMWebApi.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]

    public class CollectionController : ControllerBase 
    {
        private readonly ILogger<MemoryController> _logger;

        public CollectionController (ILogger<MemoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task GetCollections()
        {
            List<string> collections = [];

#pragma warning disable SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            
            await foreach(var collection in VectorDbService.MemoryStore.GetCollectionsAsync())
            {
                collections.Add(collection);
            }
#pragma warning restore SKEXP0026 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            
        }

        [HttpPost("create")]
        public async Task CreateCollection([FromBody] dynamic data)
        {
            var collection = data.GetProperty("collection").ToString();
            await VectorDbService.MemoryStore.CreateCollectionAsync(collection);
        }
    }
}