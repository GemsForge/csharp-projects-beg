using Microsoft.IdentityModel.Tokens;
using SecureUserConsole.manager;
using SecureUserConsole.model;
using SecureUserConsole.service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GemConnectAPI.Services.SecureUser
{
    public class ApiUserManager : IApiUserManager
    {
        private readonly IUserManager _userManager;  // Use the existing UserManager
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;  // For JWT config

        public ApiUserManager(IUserManager userManager, IUserService userService, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _userService = userService;

        }

        /// <summary>
        /// Handles user login and generates a JWT token.
        /// </summary>
        /// <param name="loginInfo">The user's login credentials.</param>
        /// <returns>A LoginResponse containing the user details and JWT token.</returns>
        public LoginResponse? LoginUser(LoginInfo loginInfo)
        {
            // Call the existing UserManager login method
            bool isLoginSuccessful = _userManager.LoginUser(loginInfo);

            if (!isLoginSuccessful)
            {
                return null;  // If login fails, return null
            }

            // Get the user details from the UserManager
            var user = _userService.GetUserByUsername(loginInfo.Username);

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Return user details and token
            return new LoginResponse
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        /// <summary>
        /// Generates a JWT token for the authenticated user.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>A JWT token.</returns>
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),  // Use userId for the Sub claim
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),    // Use userId for the NameIdentifier claim
                new Claim(ClaimTypes.Name, user.Username),                   // Use username for the Name claim
                new Claim(ClaimTypes.Role, user.Role.ToString()),            // Role claim (e.g., ADMIN or USER)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
