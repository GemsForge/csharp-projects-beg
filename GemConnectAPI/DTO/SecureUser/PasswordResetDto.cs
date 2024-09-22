using System.ComponentModel.DataAnnotations;

namespace GemConnectAPI.DTO.SecureUser
{
    /// <summary>
    /// Data Transfer Object for password reset.
    /// </summary>
    public class PasswordResetDto
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2 and 50 characters.")]
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters.")]
        public required string LastName { get; set; }

        /// <summary>
        /// Gets or sets the new password for the user.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "New password must be at least 8 characters long.")]
        public required string NewPassword { get; set; }
    }
}
