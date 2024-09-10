using System.Text.Json.Serialization;

namespace SecureUserConsole.model
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("username")]
        public required string Username { get; set; }
        [JsonPropertyName("password")]
        public required string Password { get; set; } // For simplicity; in real apps, use hashed passwords
    }

    public class LoginInfo
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
