using CommonLibrary;
using SecureUserConsole.manager;
using SecureUserConsole.model;
using SecureUserConsole.service;

namespace SecureUserConsole.ui
{
    /// <summary>
    /// Provides command-line interface (CLI) operations for user management.
    /// </summary>
    public class UserCli
    {
        private readonly IUserService _userService;
        private readonly IUserManager _userManager;
        private readonly IPasswordResetService _resetService;

        public UserCli(IUserService userService, IUserManager userManager, IPasswordResetService resetService)
        {
            _userService = userService;
            _userManager = userManager;
            _resetService = resetService;
        }

        /// <summary>
        /// Starts the CLI for user interaction.
        /// </summary>
        public void Start()
        {
            Console.Title = "Secure User Management System"; // Set the console title
            while (true)
            {
                Console.Clear();
                LogoPrinter.DisplayLogo();
                Console.WriteLine("Secure User Management System");
                Console.WriteLine("1. List Users");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Add User");
                Console.WriteLine("4. Update User");
                Console.WriteLine("5. Remove User");
                Console.WriteLine("6. Reset Password");
                Console.WriteLine("7. Exit");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ListUsers();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        AddUser();
                        break;
                    case "4":
                        UpdateUser();
                        break;
                    case "5":
                        RemoveUser();
                        break;
                    case "6":
                        ResetPassword();
                        break;
                    case "7":
                        return; // Exit the loop and close the application
                    default:
                        Console.WriteLine("Invalid choice. Please select again.");
                        break;
                }
            }
        }

        private void AddUser()
        {
            string firstName = UserInputValidator.GetValidatedFirstName("Enter First Name: ");
            string lastName = UserInputValidator.GetValidatedLastName("Enter Last Name: ");
            string email = UserInputValidator.GetValidatedEmail("Enter Email: ");
            string password = UserInputValidator.GetValidatedPassword("Enter Password: ");

            var registerInfo = new RegisterInfo
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            _userManager.RegisterUser(registerInfo);
            Console.ReadKey();
        }

        private void UpdateUser()
        {
            string username;
            User? user;

            do
            {
                // Prompt user for a valid username
                username = UserInputValidator.GetValidatedUsername("Enter Username to Update: ");

                // Check if the username exists
                user = _userService.GetUserByUsername(username);

                if (user == null)
                {
                    Console.WriteLine("User not found. Please try again.");
                }

            } while (user == null); // Continue looping until a valid user is found


            string firstName = UserInputValidator.GetValidatedFirstName(prompt: $"Enter New First Name (leave blank to keep current: {user.FirstName}): ");
            string lastName = UserInputValidator.GetValidatedLastName(prompt: $"Enter New Last Name (leave blank to keep current: {user.LastName}): ");
            string email = UserInputValidator.GetValidatedEmail(prompt: $"Enter New Email (leave blank to keep current: {user.Email}): ");
            string newUsername = UserInputValidator.GetValidatedUsername(prompt: $"Enter New Username (leave blank to keep current: {user.Username}): ");
            string password = UserInputValidator.GetValidatedPassword(prompt: $"Enter New Password (leave blank to keep current: {user.Password}): ");

            // Update fields only if new values are provided
            if (!string.IsNullOrEmpty(firstName)) user.FirstName = firstName;
            if (!string.IsNullOrEmpty(lastName)) user.LastName = lastName;
            if (!string.IsNullOrEmpty(email)) user.Email = email;
            if (!string.IsNullOrEmpty(newUsername)) user.Username = newUsername.ToLower();
            if (!string.IsNullOrEmpty(password)) user.Password = password;

            _userManager.UpdateUser(user);
            Console.ReadKey();
        }

        private void RemoveUser()
        {
            string username = UserInputValidator.GetValidatedUsername(prompt: "Enter Username to Remove: ");
            if (!string.IsNullOrEmpty(username)) _userService.RemoveUser(username);
            Console.ReadKey();
        }

        private void ListUsers()
        {
            Console.WriteLine("User List:");
            IEnumerable<User> users = _userService.GetUsers();
            foreach (var user in users)
            {
                Console.WriteLine($" Name: {user.FirstName} {user.LastName}, Username: {user.Username}, Email: {user.Email}, Password: {user.Password}");
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Handles the user login process, including checking login credentials and tracking failed attempts.
        /// </summary>
        private void Login()
        {
            string username;
            string password;
            bool isValidUser;

            do
            {
                // Re-prompt for username and password
                username = UserInputValidator.GetValidatedUsername(prompt: "Enter Username: ");
                password = UserInputValidator.GetValidatedPassword(prompt: "Enter Password: "); // Password IS case sensitive

                var loginInfo = new LoginInfo
                {
                    Username = username.ToLower(),
                    Password = password
                };

                // Try logging in the user
                isValidUser = _userManager.LoginUser(loginInfo);
                if (!isValidUser)
                {
                    // Track failed attempts and trigger password reset if necessary
                    if (_resetService.HandleFailedLogin(username))
                    {
                        Console.WriteLine("Too many failed attempts. Please verify your identity.");
                        ResetPassword();
                        return;
                    }
                }

            } while (!isValidUser); // Loop until a valid login is provided

            // Reset the failed login count on success
            _resetService.ResetFailedLoginAttempts(username);
            Console.ReadKey();
        }
        private void ResetPassword()
        {
            string username = UserInputValidator.GetValidatedUsername("Enter Username: ");
            string email = UserInputValidator.GetValidatedEmail("Enter Email: ");
            string lastName = UserInputValidator.GetValidatedLastName("Enter Last Name: ");

            // Verify user identity first
            var isVerified = _resetService.VerifyUserIdentity(username, email, lastName);
            if (isVerified)
            {
                string newPassword = UserInputValidator.GetValidatedPassword("Enter New Password: ");

                bool success = _resetService.ResetPassword(username, email, lastName, newPassword);
                if (success)
                {
                    Console.WriteLine("Password reset successful.");
                }
                else
                {
                    Console.WriteLine("Password reset failed. Please check the information and try again.");
                }
            }
            else
            {
                Console.WriteLine("Password reset failed due to verification issues.");
            }

            Console.ReadKey();
        }
    }
}
