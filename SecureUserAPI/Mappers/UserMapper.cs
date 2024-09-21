using SecureUserAPI.DTO;
using SecureUserConsole.model;

namespace SecureUserAPI.Mappers
{
    /// <summary>
    /// Provides  methods for mapping between DTOs and domain models.
    /// </summary>
    public class UserMapper : IUserMapper
    {
        /// <summary>
        /// Maps a <see cref="RegisterDto"/> to a <see cref="RegisterInfo"/> domain model.
        /// </summary>
        /// <param name="dto">The <see cref="RegisterDto"/> to map.</param>
        /// <returns>A <see cref="RegisterInfo"/> domain model.</returns>
        public RegisterInfo MapToRegisterInfo(RegisterDto dto)
        {
            return new RegisterInfo
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password
            };
        }

        /// <summary>
        /// Maps a <see cref="LoginDto"/> to a <see cref="LoginInfo"/> domain model.
        /// </summary>
        /// <param name="dto">The <see cref="LoginDto"/> to map.</param>
        /// <returns>A <see cref="LoginInfo"/> domain model.</returns>
        public LoginInfo MapToLoginInfo(LoginDto dto)
        {
            return new LoginInfo
            {
                Username = dto.Email, // Assuming email is used as the username
                Password = dto.Password
            };
        }

        /// <summary>
        /// Maps a <see cref="PasswordResetDto"/> to domain values.
        /// </summary>
        /// <param name="dto">The <see cref="PasswordResetDto"/> to map.</param>
        /// <returns>A tuple containing the domain values for password reset.</returns>
        public (string username, string email, string lastName, string newPassword) MapToPasswordReset(PasswordResetDto dto)
        {
            return (dto.Username, dto.Email, dto.LastName, dto.NewPassword);
        }
    }
}
