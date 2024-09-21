namespace SecureUserConsole.service
{
    /// <summary>
    /// Provides password reset functionality, including failed login tracking,
    /// user verification, and password updating.
    /// </summary>
    public class PasswordResetService : IPasswordResetService
    {
        private const int MaxFailedAttempts = 3;  // Maximum allowed failed login attempts
        private readonly Dictionary<string, int> _failedLoginAttempts = new();
        private readonly IPasswordUtility _passwordUtility;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordResetService"/> class.
        /// </summary>
        /// <param name="userService">The user service for accessing user data.</param>
        public PasswordResetService(IPasswordUtility passwordUtility, IUserService userService)
        {
            _passwordUtility = passwordUtility;
            _userService = userService;
        }

        /// <summary>
        /// Tracks a failed login attempt and determines if the password reset process needs to be triggered.
        /// </summary>
        /// <param name="username">The username of the user attempting to log in.</param>
        /// <returns>True if the maximum number of failed attempts has been reached, false otherwise.</returns>
        public bool HandleFailedLogin(string username)
        {
            if (_failedLoginAttempts.ContainsKey(username))
            {
                _failedLoginAttempts[username]++;
            }
            else
            {
                _failedLoginAttempts[username] = 1;
            }

            return _failedLoginAttempts[username] >= MaxFailedAttempts;
        }

        /// <summary>
        /// Resets the count of failed login attempts for a given user.
        /// </summary>
        /// <param name="username">The username of the user to reset the login attempts for.</param>
        public void ResetFailedLoginAttempts(string username)
        {
            if (_failedLoginAttempts.ContainsKey(username))
            {
                _failedLoginAttempts[username] = 0;
            }
        }

        /// <summary>
        /// Verifies if the provided username, email, and last name match a registered user.
        /// </summary>
        /// <param name="username">The username to verify.</param>
        /// <param name="email">The email to verify.</param>
        /// <param name="lastName">The last name to verify.</param>
        /// <returns><c>true</c> if the user information matches; otherwise, <c>false</c>.</returns>
        public bool VerifyUserIdentity(string username, string email, string lastName)
        {
            var user = _userService.GetUserByUsername(username);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            if (!(string.Equals(email, user.Email, StringComparison.OrdinalIgnoreCase) &&
string.Equals(lastName, user.LastName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Verification failed. Email or last name does not match.");
                return false;  // Verification failed
            }
            else
            {
                return true;  // User verified
            }
        }

        /// <summary>
        /// Handles password reset by verifying user details and updating the password.
        /// </summary>
        /// <param name="username">The username of the user attempting to reset their password.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="newPassword">The new password to set for the user.</param>
        /// <returns><c>true</c> if the password reset was successful; otherwise, <c>false</c>.</returns>
        public bool ResetPassword(string username, string email, string lastName, string newPassword)
        {
            var user = _userService.GetUserByUsername(username);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            var hashedPassword = _passwordUtility.HashPassword(newPassword);
            user.Password = hashedPassword;
            _userService.UpdateUser(user);
            Console.WriteLine("Password reset successfully.");
            return true;
        }
    }
}
