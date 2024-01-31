using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LLMWebApi.Models {
    public class Page 
    {
        [JsonPropertyName("pageNo")]
        [Required]
        public int PageNo {get; set;}

        [JsonPropertyName("paragraphs")]
        public List<string> Paragraphs {get; set;} = [];
    }
}