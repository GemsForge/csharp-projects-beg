using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SecureUserConsole.ui
{
    public static class UserInputValidator
    {
        /// <summary>
        /// Validates the username input.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <returns>Valid username if valid, otherwise prompts the user to enter a valid username.</returns>
        public static string GetValidatedUsername(string prompt)
        {
            string? username;
            do
            {
                Console.Write(prompt);
                username = Console.ReadLine();
                //If username is null or whitespace, allow it as a valid input
                if (String.IsNullOrWhiteSpace(username) && prompt.Contains("New")) //Update user prompt asks for new data
                {
                    break; //Exit loop
                }
                continue;
            } while (!IsValidUsername(username));
            return username;
        }

        /// <summary>
        /// Validates the password input.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>Valid password if valid, otherwise prompts the user to enter a valid password.</returns>
        public static string GetValidatedPassword(string prompt)
        {
            string? password;
            do
            {
                Console.Write(prompt);
                password = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(password) && prompt.Contains("New")) //Update user prompt asks for new data
                {
                    break; //Exit loop
                }
                continue;
            } while (!IsValidPassword(password));
            return password;
        }

        /// <summary>
        /// Validates the email input.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns>Valid email if valid, otherwise prompts the user to enter a valid email.</returns>
        public static string GetValidatedEmail(string prompt)
        {
            string? email;
            do
            {
                Console.Write(prompt);
                email = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(email) && prompt.Contains("New")) //Update user prompt asks for new data
                {
                    break; //Exit loop
                }
                continue;
            } while (!IsValidEmail(email));
            return email;
        }

        /// <summary>
        /// Validates the first name input.
        /// </summary>
        /// <param name="firstName">The first name to validate.</param>
        /// <returns>Valid first name if valid, otherwise prompts the user to enter a valid first name.</returns>
        public static string GetValidatedFirstName(string prompt)
        {
            string? firstName;
            do
            {
                Console.Write(prompt);
                firstName = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(firstName) && prompt.Contains("New")) //Update user prompt asks for new data
                {
                    break; //Exit loop

                }
                continue;
            } while (!IsValidName(firstName));
            return firstName;
            }

        /// <summary>
        /// Validates the last name input.
        /// </summary>
        /// <param name="lastName">The last name to validate.</param>
        /// <returns>Valid last name if valid, otherwise prompts the user to enter a valid last name.</returns>
        public static string GetValidatedLastName(string prompt)
        {
            string? lastName;
            do
            {
                Console.Write(prompt);
                lastName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(lastName) && prompt.Contains("New")) //Update user prompt asks for new data
                {
                    break; //Exit loop
                }
                continue;
            } while (!IsValidName(lastName));
            return lastName;
        }

        #region Private Validation Methods

        private static bool IsValidUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username) && username.Length >= 3;
        }

        private static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 4;
        }

        private static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter) && name.Length >= 2;
        }

        #endregion
    }
}