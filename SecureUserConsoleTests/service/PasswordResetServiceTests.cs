using Moq;
using SecureUserConsole.model;
using SecureUserConsole.service;

namespace SecureUserConsole.Tests.service
{
    [TestClass()]
    public class PasswordResetServiceTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<IPasswordUtility> _mockPasswordUtility;
        private PasswordResetService _passwordResetService;
        private User _testUser;

        [TestInitialize]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockPasswordUtility = new Mock<IPasswordUtility>();

            // Initialize a test user object
            _testUser = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                FirstName = "John",   // First name is required
                LastName = "Doe",     // Last name
                Password = "hashedpassword",  // Password is required
                Role = UserRole.USER
            };

            // Set up the mock to return the test user when the username is queried
            _mockUserService.Setup(service => service.GetUserByUsername(_testUser.Username))
                .Returns(_testUser);

            _passwordResetService = new PasswordResetService(_mockPasswordUtility.Object, _mockUserService.Object);
        }

        [TestMethod]
        public void HandleFailedLogin_ShouldReturnFalse_WhenLessThanMaxAttempts()
        {
            // Act
            bool result = _passwordResetService.HandleFailedLogin(_testUser.Username);

            // Assert
            Assert.IsFalse(result);  // Should not trigger reset on first attempt
        }

        [TestMethod]
        public void HandleFailedLogin_ShouldReturnTrue_AfterMaxFailedAttempts()
        {
            // Act
            _passwordResetService.HandleFailedLogin(_testUser.Username); // First failed attempt
            _passwordResetService.HandleFailedLogin(_testUser.Username); // Second failed attempt
            bool result = _passwordResetService.HandleFailedLogin(_testUser.Username); // Third failed attempt

            // Assert
            Assert.IsTrue(result);  // Should trigger reset after 3 failed attempts
        }

        [TestMethod]
        public void ResetFailedLoginAttempts_ShouldResetFailedAttempts()
        {
            // Simulate failed attempts
            _passwordResetService.HandleFailedLogin(_testUser.Username);
            _passwordResetService.HandleFailedLogin(_testUser.Username);

            // Act
            _passwordResetService.ResetFailedLoginAttempts(_testUser.Username);
            bool result = _passwordResetService.HandleFailedLogin(_testUser.Username); // First attempt after reset

            // Assert
            Assert.IsFalse(result);  // Failed attempts should be reset, so this should not trigger a reset
        }

        [TestMethod]
        public void VerifyUserIdentity_ShouldReturnTrue_WhenUserDetailsMatch()
        {
            // Act
            bool result = _passwordResetService.VerifyUserIdentity(_testUser.Username, _testUser.Email, _testUser.LastName);

            // Assert
            Assert.IsTrue(result);  // Should return true when user details match
        }

        [TestMethod]
        public void VerifyUserIdentity_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            string unknownUsername = "unknownuser";
            _mockUserService.Setup(service => service.GetUserByUsername(unknownUsername)).Returns((User)null);

            // Act
            bool result = _passwordResetService.VerifyUserIdentity(unknownUsername, _testUser.Email, _testUser.LastName);

            // Assert
            Assert.IsFalse(result);  // Should return false when user is not found
        }

        [TestMethod]
        public void VerifyUserIdentity_ShouldReturnFalse_WhenUserDetailsDoNotMatch()
        {
            // Act
            bool result = _passwordResetService.VerifyUserIdentity(_testUser.Username, "wrong@example.com", "Smith");

            // Assert
            Assert.IsFalse(result);  // Should return false when details do not match
        }

        [TestMethod]
        public void ResetPassword_ShouldUpdatePassword_WhenUserIsVerified()
        {
            // Arrange
            string newPassword = "newpassword";
            _mockPasswordUtility.Setup(utility => utility.HashPassword(newPassword)).Returns("hashednewpassword");

            // Act
            bool result = _passwordResetService.ResetPassword(_testUser.Username, _testUser.Email, _testUser.LastName, newPassword);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("hashednewpassword", _testUser.Password);  // Password should be updated
            _mockUserService.Verify(service => service.UpdateUser(_testUser), Times.Once);  // Ensure UpdateUser was called
        }

        [TestMethod]
        public void ResetPassword_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            string unknownUsername = "unknownuser";
            _mockUserService.Setup(service => service.GetUserByUsername(unknownUsername)).Returns((User)null);

            // Act
            bool result = _passwordResetService.ResetPassword(unknownUsername, _testUser.Email, _testUser.LastName, "newpassword");

            // Assert
            Assert.IsFalse(result);  // Should return false when user is not found
        }
    }
}