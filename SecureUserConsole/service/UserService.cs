﻿using CommonLibrary.Data;
using SecureUserConsole.model;

namespace SecureUserConsole.service
{
    /// <summary>
    /// Provides services for managing users, including creation, updating, and deletion.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepo">The repository used for loading and saving users.</param>
        public UserService(IGenericRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        #region Methods

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{User}"/> containing all users.</returns>
        public IEnumerable<User> GetUsers()
        {
            return _userRepo.GetAll();
        }

        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The <see cref="User"/> object if found; otherwise, <c>null</c>.</returns>
        public User? GetUserByUsername(string username)
        {
            return _userRepo.GetAll().FirstOrDefault(existingUser => existingUser.Username == username);
        }

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="user">The <see cref="User"/> object containing the user details.</param>
        public void AddUser(User user)
        {
            if (user != null && !UserExists(user.Email))
            {
                // Find the highest existing user ID
                int lastUserId = _userRepo.GetAll().Any() ? _userRepo.GetAll().Max(u => u.Id) : 0;

                // Assign a new sequential ID to the new user
                user.Id = lastUserId + 1;

                _userRepo.Add(user);
                Console.WriteLine("User added successfully.");
            }
            else
            {
                Console.WriteLine("User with this email already exists or invalid user.");
            }
        }

        /// <summary>
        /// Removes a user from the system by their username.
        /// </summary>
        /// <param name="username">The username of the user to remove.</param>
        public void RemoveUser(string username)
        {
            var user = GetUserByUsername(username);
            if (user != null)
            {
                _userRepo.Remove(user.Id);
                Console.WriteLine($"User {user.Username} was permanently removed.");
            }
            else
            {
                Console.WriteLine("Username not found.");
            }
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="user">The <see cref="User"/> object containing updated user details.</param>
        public void UpdateUser(User user)
        {
            if (user != null && UserExists(user.Email))
            {
                var userToUpdate = GetUserByUsername(user.Username);
                if (userToUpdate != null)
                {
                    userToUpdate.Username = user.Username;
                    userToUpdate.Email = user.Email;
                    userToUpdate.FirstName = user.FirstName;
                    userToUpdate.LastName = user.LastName;
                    userToUpdate.Password = user.Password;
                    userToUpdate.Role = user.Role;
                    _userRepo.Update(user.Id, user);
                    Console.WriteLine("User updated successfully.");
                }
            }
            else
            {
                Console.WriteLine("User does not exist.");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Checks whether a user exists based on their email.
        /// </summary>
        /// <param name="email">The email of the user to check.</param>
        /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
        private bool UserExists(string email)
        {
            return _userRepo.GetAll().Any(existingUser => existingUser.Email == email);
        }

        #endregion
    }
}
