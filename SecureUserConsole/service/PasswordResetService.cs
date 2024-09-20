using System.Net.Mail;
using System.Security.Cryptography;

namespace SecureUserConsole.service
{
    public class PasswordResetService
    {
        private const int TokenLength = 32;  // Length of the token in bytes
        private const int TokenExpirationMinutes = 15;

        /// <summary>
        /// Generates a secure token for password reset and sends it via email.
        /// </summary>
        /// <param name="userEmail">The email address of the user requesting the reset.</param>
        public void SendPasswordResetEmail(string userEmail)
        {
            // Generate a secure token
            string token = GenerateResetToken();

            // Store the token and expiration time securely
            StoreTokenInDatabase(userEmail, token, DateTime.UtcNow.AddMinutes(TokenExpirationMinutes));

            // Create the reset link
            string resetLink = $"https://yourapp.com/reset-password?token={token}";

            // Send the reset email
            var mailMessage = new MailMessage("no-reply@yourapp.com", userEmail)
            {
                Subject = "Password Reset Request",
                Body = $"Hi there!\n\nWe heard you need a password reset. Click the link below to reset your password:\n{resetLink}\n\nThis link will expire in {TokenExpirationMinutes} minutes.\n\nBest,\nYour Friendly App",
                IsBodyHtml = false
            };

            var smtpClient = new SmtpClient("smtp.yourmailprovider.com");
            smtpClient.Send(mailMessage);
        }

        /// <summary>
        /// Generates a secure token for the reset.
        /// </summary>
        /// <returns>A secure reset token as a base64 string.</returns>
        private string GenerateResetToken()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] tokenData = new byte[TokenLength];
                rng.GetBytes(tokenData);

                return Convert.ToBase64String(tokenData);  // Base64 encoding for URL compatibility
            }
        }

        /// <summary>
        /// Stores the token and its expiration in the database (hashed).
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="token">The generated reset token.</param>
        /// <param name="expiration">The expiration date/time of the token.</param>
        private void StoreTokenInDatabase(string userEmail, string token, DateTime expiration)
        {
            // Hash the token before storing it
            string hashedToken = HashToken(token);

            // Store hashedToken and expiration with the user's email in your database
            // Example: UserManager.SavePasswordResetToken(userEmail, hashedToken, expiration);
        }

        /// <summary>
        /// Hashes the token using a secure hashing algorithm.
        /// </summary>
        /// <param name="token">The token to hash.</param>
        /// <returns>The hashed token.</returns>
        private string HashToken(string token)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
                byte[] hashedBytes = sha256.ComputeHash(tokenBytes);

                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
