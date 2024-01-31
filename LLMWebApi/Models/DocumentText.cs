using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LLMWebApi.Models {
    public class DocumentText 
    {
        [JsonPropertyName("fileName")]
        [Required]
        public string FileName {get; set;} = "";
        
        [JsonPropertyName("noOfPages")]
        [Required]
        public int NoOfPages {get; set;} = 0;


        [JsonPropertyName("content")]
        [Required]
        public List<Page> Content {get; set;} = [];
    }
}