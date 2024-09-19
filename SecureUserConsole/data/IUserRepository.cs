using SecureUserConsole.model;

namespace SecureUserConsole.data
{
    public interface IUserRepository
    {
        List<User> LoadUsersFromFile();
        void SaveUsersToFile(List<User> users);
    }
}