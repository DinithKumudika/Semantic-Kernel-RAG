using System.Text.Json.Serialization;

namespace LLMWebApi.Models {
    public class Memory 
    {
        [JsonPropertyName("id")]
        public required string Id {get; set;}

        [JsonPropertyName("collection")]
        public required string Collection {get; set;}

        [JsonPropertyName("data")]
        public required string Data {get; set;}

        [JsonPropertyName("description")]
        public string? Description {get; set;} = "";

        [JsonPropertyName("metadata")]
        public string[] Metadata {get;set;} = [];
    }
}