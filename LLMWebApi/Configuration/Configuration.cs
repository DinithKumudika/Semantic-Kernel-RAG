namespace LLMWebApi.Configuration;
public sealed class Configuration 
{
    public class OpenAIConfig 
    {
        public string? ChatModelId {get; set;}
        public string? EmbeddingModelId {get; set;}
        public string? ApiKey {get; set;}
        public string? OrgId {get; set;}
    }

    public class QdrantConfig
    {
        public string? Endpoint { get; set; }
        public string? Port { get; set; }
    }
}