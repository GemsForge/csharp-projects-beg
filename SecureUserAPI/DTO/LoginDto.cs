using System.ComponentModel.DataAnnotations;

namespace SecureUserAPI.DTO
{
    /// <summary>
    /// Data Transfer Object for user login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public required string Password { get; set; }
    }
}
