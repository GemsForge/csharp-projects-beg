using SecureUserConsole.model;

namespace SecureUserConsole.service
{
    public interface IUserManager
    {
        string RegisterUser(RegisterInfo registerInfo);
        public bool LoginUser(LoginInfo loginInfo);
        void UpdateUser(User updatedUser);
        bool UserExists(string email);
    }
}