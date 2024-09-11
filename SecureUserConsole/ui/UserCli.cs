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
            while (true)
            {
                Console.Clear();
                LogoPrinter.DisplayLogo();
                Console.WriteLine("User Management CLI");
                Console.WriteLine("1. Add User");
                Console.WriteLine("2. Update User");
                Console.WriteLine("3. Remove User");
                Console.WriteLine("4. List Users");
                Console.WriteLine("5. Login");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddUser();
                        break;
                    case "2":
                        UpdateUser();
                        break;
                    case "3":
                        RemoveUser();
                        break;
                    case "4":
                        ListUsers();
                        break;
                    case "5":
                        Login();
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
            var firstName = UserInputValidator.GetValidatedFirstName("Enter First Name: ");
            var lastName = UserInputValidator.GetValidatedLastName("Enter Last Name: ");
            var email = UserInputValidator.GetValidatedEmail("Enter Email: ");
            var password = UserInputValidator.GetValidatedPassword("Enter Password: ");

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
            Console.Write("Enter Username to Update: ");
            var username = Console.ReadLine();
            var user = _userService.GetUserByUsername(username);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            var firstName = UserInputValidator.GetValidatedFirstName($"Enter New First Name (leave blank to keep current: {user.FirstName}): ");
            var lastName = UserInputValidator.GetValidatedLastName($"Enter New Last Name (leave blank to keep current: {user.LastName}): ");
            var email = UserInputValidator.GetValidatedEmail($"Enter New Email (leave blank to keep current: {user.Email}): ");
            var newUsername = UserInputValidator.GetValidatedUsername($"Enter New Last Name (leave blank to keep current: {user.Username}): ");
            var password = UserInputValidator.GetValidatedPassword($"Enter New Password (leave blank to keep current: {user.Password}): ");

            // Update fields only if new values are provided
            if (!string.IsNullOrEmpty(firstName)) user.FirstName = firstName;
            if (!string.IsNullOrEmpty(lastName)) user.LastName = lastName;
            if (!string.IsNullOrEmpty(email)) user.Email = email;
            if (!string.IsNullOrEmpty(newUsername)) user.Username = newUsername;
            if (!string.IsNullOrEmpty(password)) user.Password = password;

            _userManager.UpdateUser(user);
            Console.ReadKey();
        }

        private void RemoveUser()
        {
            Console.Write("Enter Username to Remove: ");
            var username = Console.ReadLine();
            _userService.RemoveUser(username);
            Console.ReadKey();
        }

        private void ListUsers()
        {
            var users = _userService.GetUsers();
            Console.WriteLine("User List:");
            foreach (var user in users)
            {
                Console.WriteLine($" Name: {user.FirstName} {user.LastName}, Username: {user.Username}, Email: {user.Email}, Password: {user.Password}");
            }
            Console.ReadKey();
        }

        private void Login()
        {
            var username = UserInputValidator.GetValidatedUsername("Enter Username: ");
            var password = UserInputValidator.GetValidatedPassword("Enter Password: ");

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
