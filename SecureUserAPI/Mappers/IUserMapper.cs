using SecureUserAPI.DTO;
using SecureUserConsole.model;

namespace SecureUserAPI.Mappers
{
    public interface IUserMapper
    {
        RegisterInfo MapToRegisterInfo(RegisterDto dto);
        LoginInfo MapToLoginInfo(LoginDto dto);
        (string username, string email, string lastName, string newPassword) MapToPasswordReset(PasswordResetDto dto);
        User MapToUpdatedUser(UpdateUserDto dto, User existingUser);
    }
}