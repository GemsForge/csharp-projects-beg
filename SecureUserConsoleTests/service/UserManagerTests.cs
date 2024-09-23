using Moq;
using SecureUserConsole.model;
using SecureUserConsole.service;

namespace SecureUserConsoleTests.service
{
    [TestClass]
    public class UserManagerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IPasswordUtility> _passwordUtilityMock;
        private UserManager _userManager;

        [TestInitialize]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _passwordUtilityMock = new Mock<IPasswordUtility>();
            _userManager = new UserManager(_userServiceMock.Object, _passwordUtilityMock.Object);
        }

        [TestMethod]
        public void RegisterUser_ShouldAddUser_WhenUserDoesNotExist()
        {
            // Arrange
            var registerInfo = new RegisterInfo
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
            };
            

            _userServiceMock.Setup(s => s.GetUsers()).Returns(new List<User>());
            _passwordUtilityMock.Setup(p => p.HashPassword(registerInfo.Password)).Returns("hashedPassword");

            // Act
            _userManager.RegisterUser(registerInfo);

            // Assert
            _userServiceMock.Verify(s => s.AddUser(It.Is<User>(u =>
                u.FirstName == registerInfo.FirstName &&
                u.LastName == registerInfo.LastName &&
                u.Email == registerInfo.Email &&
                u.Password == "hashedPassword" &&
                u.Username == "doejoh")), Times.Once);
        }

        [TestMethod]
        public void RegisterUser_ShouldNotAddUser_WhenUserAlreadyExists()
        {
            // Arrange
            var registerInfo = new RegisterInfo
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123"
            };

            var existingUser = new User { FirstName = "John", LastName = "Doe", Email = registerInfo.Email, Password = "existingPassword", Username = "doejoh", Role = UserRole.USER };
            _userServiceMock.Setup(s => s.GetUsers()).Returns(new List<User> { existingUser });

            // Act
            _userManager.RegisterUser(registerInfo);

            // Assert
            _userServiceMock.Verify(s => s.AddUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void LoginUser_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var loginInfo = new LoginInfo
            {
                Username = "doejoh",
                Password = "Password123"
            };

            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Username = loginInfo.Username,
                Password = "hashedPassword",
                Role = UserRole.USER
            };

            _userServiceMock.Setup(s => s.GetUserByUsername(loginInfo.Username)).Returns(user);
            _passwordUtilityMock.Setup(p => p.VerifyPassword( loginInfo.Password, user.Password)).Returns(true);

            // Act
            var result = _userManager.LoginUser(loginInfo);

            // Assert
             Assert.IsTrue(result);
        }

        [TestMethod]
        public void LoginUser_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginInfo = new LoginInfo
            {
                Username = "doejoh",
                Password = "Password123"
            };

            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@email.com",
                Username = loginInfo.Username,
                Password = "hashedPassword",
                Role = UserRole.USER
            };

            _userServiceMock.Setup(s => s.GetUserByUsername(loginInfo.Username)).Returns(user);
            _passwordUtilityMock.Setup(p => p.VerifyPassword(loginInfo.Password, user.Password)).Returns(false);

            // Act
            var result = _userManager.LoginUser(loginInfo);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateUser_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var updatedUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@newexample.com",
                Password = "NewPassword123",
                Username = "doejoh",
                Role = UserRole.USER
            };

            var existingUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "oldPassword",
                Username = updatedUser.Username,
                Role = UserRole.USER
            };

            _userServiceMock.Setup(s => s.GetUserByUsername(updatedUser.Username)).Returns(existingUser);
            _passwordUtilityMock.Setup(p => p.HashPassword(updatedUser.Password)).Returns("newHashedPassword");

            // Act
            _userManager.UpdateUser(updatedUser);

            // Assert
            _userServiceMock.Verify(s => s.UpdateUser(It.Is<User>(u =>
                u.FirstName == updatedUser.FirstName &&
                u.LastName == updatedUser.LastName &&
                u.Email == updatedUser.Email &&
                u.Password == "newHashedPassword")), Times.Once);

        }

        [TestMethod]
        public void UpdateUser_ShouldNotUpdateUser_WhenUserDoesNotExist()
        {
            // Arrange
            var updatedUser = new User
            {
                Username = "doejoh",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@newexample.com",
                Password = "NewPassword123",
                Role = UserRole.USER
            };

            _userServiceMock.Setup(s => s.GetUserByUsername(updatedUser.Username)).Returns((User)null);

            // Act
            _userManager.UpdateUser(updatedUser);

            // Assert
            _userServiceMock.Verify(s => s.UpdateUser(It.IsAny<User>()), Times.Never);
        }
    }
}
