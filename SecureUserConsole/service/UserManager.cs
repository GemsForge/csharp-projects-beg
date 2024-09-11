using SecureUserConsole.model;

namespace SecureUserConsole.service
{
    /// <summary>
    /// Manages user-specific operations such as registration and login.
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="userService">The <see cref="UserService"/> instance used for CRUD operations.</param>
        public UserManager(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerInfo">The <see cref="RegisterInfo"/> object containing registration details.</param>
        public void RegisterUser(RegisterInfo registerInfo)
        {
            if (registerInfo != null && !UserExists(registerInfo.Email))
            {
                User newUser = new()
                {
                    FirstName = registerInfo.FirstName,
                    LastName = registerInfo.LastName,
                    Email = registerInfo.Email,
                    Password = registerInfo.Password,
                    Username = CreateUniqueUsername(registerInfo.FirstName, registerInfo.LastName)
                };
                _userService.AddUser(newUser);
                Console.WriteLine("User registered successfully.");
            }
            else
            {
                Console.WriteLine("User with this email already exists or invalid registration details.");
            }
        }

        /// <summary>
        /// Logs in a user by checking their credentials.
        /// </summary>
        /// <param name="loginInfo">The <see cref="LoginInfo"/> object containing login credentials.</param>
        /// <returns><c>true</c> if login is successful; otherwise, <c>false</c>.</returns>
        public bool LoginUser(LoginInfo loginInfo)
        {
            var user = _userService.GetUserByUsername(loginInfo.Username);
            if (user != null && user.Password == loginInfo.Password)
            {
                Console.WriteLine("Login successful.");
                return true;
            }
            Console.WriteLine("Invalid username or password.");
            return false;
        }

        #region Helper Methods

        /// <summary>
        /// Creates a unique username by concatenating a substring of the first name with the last name.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <returns>A unique username.</returns>
        private static string CreateUniqueUsername(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("First name and last name cannot be null or empty.");

            // Use the first 3 characters of the first name and concatenate with the last name
            string firstNamePart = firstName.Length > 3 ? firstName.Substring(0, 3) : firstName;
            string username = $"{lastName.ToLower()}{firstNamePart.ToLower()}";

            return username;
        }

        /// <summary>
        /// Checks whether a user exists based on their email.
        /// </summary>
        /// <param name="email">The email of the user to check.</param>
        /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
        private bool UserExists(string email)
        {
            return _userService.GetUsers().Any(existingUser => existingUser.Email == email);
        }

        #endregion
    }
}
