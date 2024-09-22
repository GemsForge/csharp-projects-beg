using SecureUserConsole.model;

namespace GemConnectAPI.Services.SecureUser
{
    public interface IApiUserManager
    {
        LoginResponse? LoginUser(LoginInfo loginInfo);
    }
}
