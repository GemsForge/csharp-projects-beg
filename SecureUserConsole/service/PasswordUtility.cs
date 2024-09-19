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

            // Use the latest overload of Rfc2898DeriveBytes with SHA-256
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                // Derive the hash of the password
                var hash = pbkdf2.GetBytes(HashSize);

                // Combine salt and hash into a single array
                var hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                // Return as a Base64 string
                return Convert.ToBase64String(hashBytes);
            }
        }


        /// <summary>
        /// Verifies a password against a stored hashed password.
        /// </summary>
        /// <param name="providedPassword">The password to verify.</param>
        /// <param name="storedHash">The stored hashed password.</param>
        /// <returns>True if the password is correct, otherwise false.</returns>
        public bool VerifyPassword(string providedPassword, string storedHash)
        {
            // Decode the stored hash from Base64
            var hashBytes = Convert.FromBase64String(storedHash);

            // Check if the hashBytes length matches what is expected (SaltSize + HashSize)
            if (hashBytes.Length != SaltSize + HashSize)
            {
                throw new ArgumentException("Stored hash length is invalid.");
            }

            // Extract the salt from the stored hash
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Extract the stored password hash (actual hash)
            var storedHashBytes = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, storedHashBytes, 0, HashSize);

            // Compute the hash of the provided password using the same salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var computedHash = pbkdf2.GetBytes(HashSize);

                // Compare each byte of the computed hash with the stored hash
                for (int i = 0; i < HashSize; i++)
                {
                    if (computedHash[i] != storedHashBytes[i])
                    {
                        return false; // Hashes don't match, so password is invalid
                    }
                }
            }

            return true; // Password is valid
        }

    }
}
