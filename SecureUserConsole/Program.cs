using SecureUserConsole.data;
using SecureUserConsole.service;
using SecureUserConsole.ui;

namespace SecureUserConsole;

public class Program
{
    private static void Main(string[] args)
    {
        string filePath = @"C:\Users\Diamond R. Brown\OneDrive\Gem.Professional 🎖️\02 💻 GemsCode\Git Repositories\CSharpProjects\SecureUserConsole\data\Users.json";
        IUserService userService = new UserService(new UserRepository(filePath));
        IPasswordUtility passwordUtility = new PasswordUtility();
        IPasswordResetService passwordReset = new PasswordResetService(passwordUtility, userService);
        IUserManager userManager = new UserManager(userService, passwordUtility);
        UserCli userConsole = new(userService, userManager, passwordReset);
        userConsole.Start();
    }
}