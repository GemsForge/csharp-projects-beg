using System.ComponentModel.DataAnnotations;

namespace GemConnectAPI.DTO.SecureUser
{
    /// <summary>
    /// Data Transfer Object (DTO) for user login.
    /// Used to transfer login credentials between the client and server.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// This field is required and must be between 3 and 50 characters long.
        /// </summary>
        /// <example>john_doe</example>
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// This field is required and must be at least 8 characters long.
        /// </summary>
        /// <example>Password123!</example>
        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public required string Password { get; set; }
    }
}
