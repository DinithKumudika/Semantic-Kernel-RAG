using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LLMWebApi.Models {
    public class Memory 
    {

        [JsonPropertyName("id")]
        [Required]
        public required string Id {get; set;}

        [JsonPropertyName("uid")]
        [Required]
        public required string UId {get; set;}

        [JsonPropertyName("collection")]
        [Required]
        public required string Collection {get; set;}

        [JsonPropertyName("description")]
        [Required]
        public required string Description {get; set;}

        [JsonPropertyName("metadata")]
        [Required]
        public required string[] Metadata {get;set;}

        [JsonPropertyName("createdAt")]
        [Required]
        public required string CreatedAt {get;set;}
    }
}