using System.Text.Json.Serialization;

namespace Auth_API.Models.Request
{
    public class LogInRequestModel
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
