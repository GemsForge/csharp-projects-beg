using CommonLibrary;
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

        public UserCli(IUserService userService, IUserManager userManager)
        {
            _userService = userService;
            _userManager = userManager;
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
                Console.WriteLine("6. Exit");
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
            var users = _userService.GetUsers();
            foreach (var user in users)
            {
                Console.WriteLine($" Name: {user.FirstName} {user.LastName}, Username: {user.Username}, Email: {user.Email}, Password: {user.Password}");
            }
            Console.ReadKey();
        }

        private void Login()
        {
            string username = UserInputValidator.GetValidatedUsername(prompt: "Enter Username: ");
            string password = UserInputValidator.GetValidatedPassword(prompt: "Enter Password: "); //Password IS case sensitive

            var loginInfo = new LoginInfo
            {
                Username = username,
                Password = password
            };
            _userManager.LoginUser(loginInfo);
            Console.ReadKey();
        }
    }
}
