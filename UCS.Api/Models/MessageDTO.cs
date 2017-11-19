using Newtonsoft.Json;
using System.Web;

namespace UCS.Api.Models
{
    public class MessageDTO
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}