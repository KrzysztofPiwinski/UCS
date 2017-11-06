using Newtonsoft.Json;

namespace UCS.Web.Models.DTOs
{
    public class StudentResponseDTO
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("fisrt_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}