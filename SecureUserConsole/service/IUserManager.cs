using SecureUserConsole.model;

namespace SecureUserConsole.service
{
    public interface IUserManager
    {
        void RegisterUser(RegisterInfo registerInfo);
        public bool LoginUser(LoginInfo loginInfo);
    }
}