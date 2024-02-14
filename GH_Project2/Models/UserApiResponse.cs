using System.Text.Json.Serialization;

namespace GH_Project2.Models
{
    public class UserApiResponse
    {
        [JsonPropertyName("")]
        public List<User>? Data { get; set; }
    }
}
