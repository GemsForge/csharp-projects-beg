using System.ComponentModel.DataAnnotations;

namespace SecureUserConsole.dto
{

    /// <summary>
    /// Data Transfer Object for user registration.
    /// </summary>
    public class UserRegisterDto
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [Required]
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [Required]
        public required string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        [Required]
        public required string Password { get; set; }
    }
}