namespace SecureUserConsole.service
{
    public interface IPasswordUtility
    {
        string HashPassword(string password);
        bool VerifyPassword(string providedPassword, string storedHash);
    }

}