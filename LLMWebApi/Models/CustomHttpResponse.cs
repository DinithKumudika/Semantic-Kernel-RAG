using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LLMWebApi.Models {

    public class CustomHttpResponse 
    {
        [JsonPropertyName("status")]
        [Required]
        public required int Status {get; set;}

        [JsonPropertyName("message")]
        public required string Message {get; set;}
    }
}