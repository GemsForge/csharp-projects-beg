using System.ComponentModel.DataAnnotations;

namespace GemConnectAPI.DTO.SecureUser
{
    /// <summary>
    /// Data Transfer Object (DTO) for handling password reset requests.
    /// This DTO is used to transfer necessary data for a user to reset their password.
    /// </summary>
    public class PasswordResetDto
    {
        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// Must be a valid email address format.
        /// </summary>
        /// <example>user@example.com</example>
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the username of the user requesting the password reset.
        /// The username must be between 2 and 50 characters.
        /// </summary>
        /// <example>john_doe</example>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2 and 50 characters.")]
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user requesting the password reset.
        /// The last name must be between 2 and 50 characters.
        /// </summary>
        /// <example>Doe</example>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters.")]
        public required string LastName { get; set; }

        /// <summary>
        /// Gets or sets the new password for the user.
        /// The new password must be at least 8 characters long.
        /// </summary>
        /// <example>NewPassw0rd!</example>
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "New password must be at least 8 characters long.")]
        public required string NewPassword { get; set; }
    }
}
