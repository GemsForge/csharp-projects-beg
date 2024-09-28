using System.ComponentModel.DataAnnotations;

namespace GemConnectAPI.DTO.SecureUser
{
    /// <summary>
    /// Data Transfer Object (DTO) for user registration.
    /// Used to transfer data when a new user registers an account.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// The first name must be between 2 and 50 characters.
        /// </summary>
        /// <example>John</example>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters.")]
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// The last name must be between 2 and 50 characters.
        /// </summary>
        /// <example>Doe</example>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters.")]
        public required string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// The email must be a valid email address.
        /// </summary>
        /// <example>john.doe@example.com</example>
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user.
        /// The password must be at least 8 characters long.
        /// </summary>
        /// <example>P@ssw0rd!</example>
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public required string Password { get; set; }
    }
}
