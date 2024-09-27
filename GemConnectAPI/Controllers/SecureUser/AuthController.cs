using GemConnectAPI.DTO.SecureUser;
using GemConnectAPI.Mappers.SecureUser;
using GemConnectAPI.Services.SecureUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureUserConsole.manager;
using SecureUserConsole.service;

namespace GemConnectAPI.Controllers.SecureUser
{

    /// <summary>
    /// Handles user authentication operations such as login and password reset.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IApiUserManager _apiUserManager;
        private readonly IPasswordResetService _passwordResetService;
        private readonly IUserMapper _userMapper;

        public AuthController(IUserManager userManager, IApiUserManager apiUserManager, IPasswordResetService passwordResetService, IUserMapper userMapper)
        {
            _userManager = userManager;
            _apiUserManager = apiUserManager;
            _passwordResetService = passwordResetService;
            _userMapper = userMapper;
        }

        /// <summary>
        /// Logs a user into the system.
        /// </summary>
        /// <param name="loginDto">An object containing the user's login credentials.</param>
        /// <returns>Returns a 200 OK status if login is successful, or a 401 Unauthorized status if the credentials are invalid.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // 400 Bad Request for invalid input
            }

            var loginInfo = _userMapper.MapToLoginInfo(loginDto);
            var loginResponse = _apiUserManager.LoginUser(loginInfo);
            if (loginResponse != null)
            {
                _passwordResetService.ResetFailedLoginAttempts(loginInfo.Username);
                return Ok($"Login successful.\nUsername: {loginResponse.Username}\n Role: {loginResponse.Role}\n Token: {loginResponse.Token}\n ");  // 200 OK
            }
            else
            {
                if (_passwordResetService.HandleFailedLogin(loginInfo.Username))
                {
                    return StatusCode(403, "Too many failed attempts. Password reset required.");  // 403 Forbidden
                }
                return Unauthorized("Invalid username or password.");  // 401 Unauthorized
            }
        }

        /// <summary>
        /// Resets a user's password.
        /// </summary>
        /// <param name="passwordResetDto">An object containing the necessary details for resetting the user's password.</param>
        /// <returns>Returns a 200 OK status if the password is successfully reset, or a 400 Bad Request status if verification fails.</returns>
        [HttpPost("password-reset")]
        public IActionResult ResetPassword([FromBody] PasswordResetDto passwordResetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // 400 Bad Request for invalid input
            }

            var (username, email, lastName, newPassword) = _userMapper.MapToPasswordReset(passwordResetDto);
            if (_passwordResetService.VerifyUserIdentity(username, email, lastName))
            {
                if (_passwordResetService.ResetPassword(username, email, lastName, newPassword))
                {
                    return Ok("Password reset successful.");  // 200 OK
                }
                return StatusCode(500, "Password reset failed due to server error.");  // 500 Internal Server Error
            }

            return BadRequest("Password reset failed due to incorrect verification details.");  // 400 Bad Request
        }
        /// <summary>
        /// Dashboard for users and admins.
        /// </summary>
        [Authorize(Policy = "USER")]
        [HttpGet("dashboard")]
        public IActionResult UserDashboard()
        {
            var role = User.IsInRole("ADMIN") ? "Admin" : "User";
            return Ok($"Welcome to your {role} dashboard.");
        }
    }
}