using SecureUserConsole.data;
using SecureUserConsole.model;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string filePath = @"C:\Users\Diamond R. Brown\OneDrive\Gem.Professional 🎖️\02 💻 GemsCode\Git Repositories\CSharpProjects\SecureUserConsole\data\Users.json";
        IUserRepository userRepo = new UserRepository(filePath);

        var users = userRepo.LoadUsersFromFile();

    }
}