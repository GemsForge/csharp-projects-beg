using SecureUserConsole.model;

namespace SecureUserConsole.manager
{
    public interface IUserManager
    {
        string RegisterUser(RegisterInfo registerInfo);
        public bool LoginUser(LoginInfo loginInfo);
        void UpdateUser(User updatedUser);
    }
}