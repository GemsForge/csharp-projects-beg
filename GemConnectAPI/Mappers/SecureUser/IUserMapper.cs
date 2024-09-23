using GemConnectAPI.DTO.SecureUser;
using SecureUserConsole.model;

namespace GemConnectAPI.Mappers.SecureUser
{
    public interface IUserMapper
    {
        RegisterInfo MapToRegisterInfo(RegisterDto dto);
        LoginInfo MapToLoginInfo(LoginDto dto);
        (string username, string email, string lastName, string newPassword) MapToPasswordReset(PasswordResetDto dto);
        User MapToUpdatedUser(UpdateUserDto dto, User existingUser);
    }
}