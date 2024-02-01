using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LLMWebApi.Models {
    public class ChatMessage 
    {
        [JsonPropertyName("id")]
        public int Id {get; set;}
        [JsonPropertyName("content")]
        [Required]
        public string? Content {get; set;}
        [JsonPropertyName("role")]
        [Required]
        public string? Role {get; set;}
    }
}