using SecureUserConsole.service;

namespace SecureUserConsoleTests.service
{
    [TestClass()]
    public class PasswordUtilityTests
    {
        private IPasswordUtility _passwordUtility;

        [TestInitialize]
        public void Setup()
        {
            _passwordUtility = new PasswordUtility();
        }

        [TestMethod]
        public void HashPassword_ShouldReturnDifferentHashForDifferentSalts()
        {
            // Arrange
            var password = "TestPassword123";
            var hash1 = _passwordUtility.HashPassword(password);
            var hash2 = _passwordUtility.HashPassword(password);

            // Act & Assert
            // Ensure that hashing the same password results in different hashes
            Assert.AreNotEqual(hash1, hash2, "Hashing the same password with different salts should result in different hashes.");
        }

        [TestMethod]
        public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "TestPassword123";
            var hashedPassword = _passwordUtility.HashPassword(password);

            // Act
            var isValid = _passwordUtility.VerifyPassword(password, hashedPassword);

            // Assert
            Assert.IsTrue(isValid, "The password verification should return true for the correct password.");
        }

        [TestMethod]
        public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var password = "TestPassword123";
            var incorrectPassword = "WrongPassword456";
            var hashedPassword = _passwordUtility.HashPassword(password);

            // Act
            var isValid = _passwordUtility.VerifyPassword(incorrectPassword, hashedPassword);

            // Assert
            Assert.IsFalse(isValid, "The password verification should return false for an incorrect password.");
        }
    }
}
