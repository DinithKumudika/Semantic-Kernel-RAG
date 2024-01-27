using LLMWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LLMWebApi.Controllers 
{

    [ApiController]
    [Route("api/chatHistory")]
    public class ChatHistoryController: ControllerBase 
    {
        // [HttpPost]
        // public async Task<IActionResult> CreateChatSessionAsync([FromBody] CreateChat chatParams)
        // {
        //     if(chatParams.Title == null){
        //         return this.BadRequest("Chat title cannot be null");
        //     }
        // }
    }
}