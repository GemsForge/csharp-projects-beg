namespace GemConnectAPI.DTO.SecureUser
{
    /// <summary>
    /// Data Transfer Object (DTO) for updating user information.
    /// Used to transfer data when a user updates their account details.
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// Gets or sets the updated first name of the user.
        /// This field is optional.
        /// </summary>
        /// <example>John</example>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the updated last name of the user.
        /// This field is optional.
        /// </summary>
        /// <example>Doe</example>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the updated email address of the user.
        /// This field is optional.
        /// </summary>
        /// <example>john.doe@example.com</example>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the updated username of the user.
        /// This field is optional.
        /// </summary>
        /// <example>johnnydoe</example>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the updated password of the user.
        /// This field is optional.
        /// </summary>
        /// <example>P@ssw0rdNew!</example>
        public string? Password { get; set; }
    }
}
