﻿using SecureUserConsole.model;
using SecureUserConsole.service;

namespace SecureUserConsole.manager
{
    /// <summary>
    /// Manages user-specific operations such as registration and login.
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly IUserService _userService;
        private readonly IPasswordUtility _passwordUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="userService">The <see cref="UserService"/> instance used for CRUD operations.</param>
        public UserManager(IUserService userService, IPasswordUtility passwordUtility)
        {
            _userService = userService;
            _passwordUtility = passwordUtility;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerInfo">The <see cref="RegisterInfo"/> object containing registration details.</param>
        public string RegisterUser(RegisterInfo registerInfo)
        {
            if (registerInfo != null && !_userService.GetUsers().Any(u => u.Email == registerInfo.Email)) // Check if user already exists in service
            {
                var hashedPassword = _passwordUtility.HashPassword(registerInfo.Password);
                User newUser = new()
                {
                    FirstName = registerInfo.FirstName,
                    LastName = registerInfo.LastName,
                    Email = registerInfo.Email,
                    Password = hashedPassword,
                    Username = CreateUniqueUsername(registerInfo.FirstName, registerInfo.LastName).ToLower(),
                    Role = UserRole.USER
                };

                _userService.AddUser(newUser); // Add the new user through the service (which handles persistence)
                return newUser.Username;  // Return the generated username
            }
            else
            {
                throw new InvalidOperationException("User with this email already exists or invalid registration details.");
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
            if (user != null && _passwordUtility.VerifyPassword(loginInfo.Password, user.Password))
            {
                Console.WriteLine("Login successful.");
                return true;
            }
            Console.WriteLine("Invalid username or password.");
            return false;
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="updatedUser">The <see cref="User"/> object containing updated user details.</param>
        public void UpdateUser(User updatedUser)
        {
            if (updatedUser != null && !string.IsNullOrEmpty(updatedUser.Username))
            {
                var existingUser = _userService.GetUserByUsername(updatedUser.Username);
                if (existingUser != null)
                {
                    var hashedPassword = _passwordUtility.HashPassword(updatedUser.Password);
                    existingUser.FirstName = updatedUser.FirstName;
                    existingUser.LastName = updatedUser.LastName;
                    existingUser.Email = updatedUser.Email;
                    existingUser.Password = hashedPassword;

                    _userService.UpdateUser(existingUser); // Update user through the service (which persists changes)
                    Console.WriteLine("User updated successfully.");
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }
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

        #endregion
    }
}
