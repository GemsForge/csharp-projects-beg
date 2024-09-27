using System.Text.Json.Serialization;

namespace SecureUserConsole.model
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the concatenated username for the user (last name + first name).
        /// </summary>
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [JsonPropertyName("password")]
        public string? Password { get; set; }


        /// <summary>
        /// Gets or sets the user's role.
        /// </summary>
        [JsonPropertyName("role")]
        [JsonConverter(typeof(JsonStringEnumConverter))]  // For System.Text.Json
        public UserRole? Role { get;  set; }
    }
    public class UserWrapper
    {
        [JsonPropertyName("users")]
        public List<User>? Users { get; set; } = [];
    }

    /// <summary>
    /// Represents login information for a user.
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public required string Password { get; set; }
    }

    ///<summary> Represent registration information for a user </summary>
    public class RegisterInfo
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
