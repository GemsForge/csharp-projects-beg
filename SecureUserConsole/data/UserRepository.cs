using SecureUserConsole.model;
using System.Text;
using System.Text.Json;

namespace SecureUserConsole.data
{
    public class UserRepository : IUserRepository
    {
        private readonly string _filePath;

        public UserRepository(string filePath)
        {
            _filePath = filePath;
        }

        ///<summary>
        /// Normalizes the file path to handle special characters and Unicode correctly,
        /// and gets the full absolute path.
        /// </summary>
        /// <param name="path">The file path to normalize and get the full path.</param>
        /// <returns>The normalized and full absolute file path.</returns>
        public static string NormalizeAndGetFullPath(string path)
        {
            try
            {
                // Normalize the path to Form C (Canonical Decomposition followed by Canonical Composition)
                path = path.Normalize(NormalizationForm.FormC);

                // Get the full absolute path
                return Path.GetFullPath(path);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine($"Invalid path provided: {ae.Message}");
                throw;  // Rethrow the exception to be handled by the caller or test case
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing the file path: {ex.Message}");
                throw;
            }
        }

        ///<summary>
        ///Loads users from the JSON file
        ///</summary>
        ///<returns>A list of users from the file</returns>
        ///
        public List<User> LoadUsersFromFile()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Users file not found. Initializing with a pre-populated list.");
                var users = InitializeDefaultUsers();
                return users;
            }
            //Encapsulate in try_catch block
            try
            {
                //Read the JSON file
                string json = File.ReadAllText(_filePath);
                //Log the JSON content for debugging purposes
                Console.WriteLine("JSON content read from file:");
                Console.WriteLine(json);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                //Return deserialize JSON into a list of User objects 
                List<User> userList = JsonSerializer.Deserialize<List<User>>(json, options);
                Console.WriteLine($"Users loaded successfully from the file. Total users: {userList}");
                return userList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading users from file: {ex.Message}");
                return new List<User>();
            }
        }

        private static List<User> InitializeDefaultUsers()
        {
            // DEBUG: Initialize list of test users
            return new List<User>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Smith",
                    Username = "smithalice",
                    Email = "alice.smith@example.com",
                    Password = "passwordAlice123" // In a real application, this should be hashed
                },
                new()
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Username = "johnsonbob",
                    Email = "bob.johnson@example.com",
                    Password = "passwordBob456" // In a real application, this should be hashed
                },
                new()
                {
                    Id = 3,
                    FirstName = "Charlie",
                    LastName = "Brown",
                    Username = "browcharlie",
                    Email = "charlie.brown@example.com",
                    Password = "passwordCharlie789" // In a real application, this should be hashed
                }
            };
        }


        ///<summary>
        ///Saves users to JSON File
        /// </summary>
        /// <param name="users">The list of users to save.</param>
        public void SaveUsersToFile(List<User> users)
        {
            try
            {
                //Configure JSON options
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(users, options);
                File.WriteAllText(_filePath, json);
                Console.WriteLine("Users saved successfully to this file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users to file: {ex.Message}");
                throw; //Rethrow the exception to allow the test to catch it
            }
        }
    }
}
