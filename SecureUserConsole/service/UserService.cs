using System.Collections.Generic;
using System.Linq;
using SecureUserConsole.data;
using SecureUserConsole.model;

namespace SecureUserConsole.service
{
    /// <summary>
    /// Provides services for managing users, including creation, updating, and deletion.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly List<User> _users;
        private readonly IUserRepository _userRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepo">The repository used for loading and saving users.</param>
        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _users = _userRepo.LoadUsersFromFile();
        }

        #region Methods

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{User}"/> containing all users.</returns>
        public IEnumerable<User> GetUsers()
        {
            return _users;
        }

        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The <see cref="User"/> object if found; otherwise, <c>null</c>.</returns>
        public User? GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(existingUser => existingUser.Username == username);
        }

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="user">The <see cref="User"/> object containing the user details.</param>
        public void AddUser(User user)
        {
            if (user != null && !UserExists(user.Email))
            {
                _users.Add(user);
                _userRepo.SaveUsersToFile(_users);
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
                _users.Remove(user);
                _userRepo.SaveUsersToFile(_users);
                Console.WriteLine($"User {user.Username} was permanately removed.");
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
                    _userRepo.SaveUsersToFile(_users);
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
            return _users.Any(existingUser => existingUser.Email == email);
        }

        #endregion
    }
}
