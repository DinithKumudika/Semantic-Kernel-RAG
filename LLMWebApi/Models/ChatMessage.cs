using System.Text.Json.Serialization;

namespace LLMWebApi.Models {
    public class ChateMessage 
    {
        [JsonPropertyName("user_id")]
        public string? UserId {get; set;}
        [JsonPropertyName("username")]
        public string? Username {get; set;}
        [JsonPropertyName("chat_id")]
        public string? ChatId {get; set;}
        [JsonPropertyName("message")]
        public string? Message {get; set;}
        [JsonPropertyName("message_type")]
        public string? MessageType {get; set;}
    }
}