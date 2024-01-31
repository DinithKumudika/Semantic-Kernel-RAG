using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LLMWebApi.Models {
    public class DocumentDirectory 
    {
        [JsonPropertyName("directoryName")]
        [Required]
        public required string DirectoryName {get; set;}

        [JsonPropertyName("collection")]
        public required string Collection {get; set;}

        [JsonPropertyName("fileData")]
        public required List<Document> FileData {get; set;}
    }
}