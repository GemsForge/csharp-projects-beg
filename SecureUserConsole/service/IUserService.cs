using SecureUserConsole.model;

namespace SecureUserConsole.service
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User? GetUserByUsername(string username);
        void AddUser(User user);
        void RemoveUser(string username);
        void UpdateUser(User user);

    }
}