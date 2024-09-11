using System;
using System.Security.Cryptography;

namespace SecureUserConsole.service
{
    public class PasswordUtility : IPasswordUtility
    {
        private const int SaltSize = 16; // Size of the salt in bytes
        private const int HashSize = 20; // Size of the hash in bytes
        private const int Iterations = 10000; // Number of iterations for the PBKDF2 algorithm

        /// <summary>
        /// Hashes a password with a salt using PBKDF2.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The salted and hashed password.</returns>
        public string HashPassword(string password)
        {
            // Generate a random salt
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var hash = pbkdf2.GetBytes(HashSize);
                var hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Verifies a password against a stored hashed password.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="storedHash">The stored hashed password.</param>
        /// <returns>True if the password is correct, otherwise false.</returns>
        public bool VerifyPassword(string providedPassword, string storedHash)
        {
            var hashBytes = Convert.FromBase64String(storedHash);
            var salt = new byte[SaltSize];
            var storedHashBytes = new byte[HashSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, storedHashBytes, 0, HashSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, Iterations))
            {
                var hash = pbkdf2.GetBytes(HashSize);
                for (int i = 0; i < HashSize; i++)
                {
                    if (hash[i] != storedHashBytes[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
