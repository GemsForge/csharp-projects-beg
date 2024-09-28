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
        /// <example>1</example>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        /// <example>John</example>
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        /// <example>Doe</example>
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the concatenated username for the user.
        /// </summary>
        /// <example>johndoe</example>
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <example>johndoe@example.com</example>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        /// <example>P@ssw0rd</example>
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the user's role in the system.
        /// </summary>
        /// <example>ADMIN</example>
        [JsonPropertyName("role")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole? Role { get; set; }
    }

    /// <summary>
    /// Represents a wrapper for a list of users.
    /// </summary>
    public class UserWrapper
    {
        /// <summary>
        /// Gets or sets the list of users in the system.
        /// </summary>
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
        /// <example>johndoe</example>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        /// <example>P@ssw0rd</example>
        public required string Password { get; set; }
    }

    /// <summary>
    /// Represents registration information for a user.
    /// </summary>
    public class RegisterInfo
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        /// <example>John</example>
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        /// <example>Doe</example>
        public required string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <example>johndoe@example.com</example>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        /// <example>P@ssw0rd</example>
        public required string Password { get; set; }
    }
}
