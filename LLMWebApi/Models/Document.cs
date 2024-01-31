using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LLMWebApi.Models {
    public class Document 
    {
        [JsonPropertyName("fileName")]
        [Required]
        public required string FileName {get; set;}

        [JsonPropertyName("collection")]
        public string? Collection {get; set;}


        [JsonPropertyName("description")]
        public string Description {get; set;} = "";

        [JsonPropertyName("metadata")]
        public string[] Metadata {get;set;} = [];
    }
}