using System.Text.Json.Serialization;

namespace LLMWebApi.Models {
    public class CreateChat 
    {
        [JsonPropertyName("title")]
        public string? Title {get; set;}
    }
}