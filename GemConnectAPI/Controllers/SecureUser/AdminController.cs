using GemConnectAPI.DTO.SecureUser;
using GemConnectAPI.Mappers.SecureUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureUserConsole.model;
using SecureUserConsole.service;

/// <summary>
/// Handles operations related to user management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUserManager _userManager;
    private readonly IUserService _userService;
    private readonly IUserMapper _userMapper;

    public AdminController(IUserManager userManager, IUserService userService, IUserMapper userMapper)
    {
        _userManager = userManager;
        _userService = userService;
        _userMapper = userMapper;
    }

    /// <summary>
    /// Gets a list of all users (Admin access only).
    /// </summary>
    /// <returns>Returns a list of users or a 404 status if no users are found.</returns>
    [Authorize(Policy = "ADMIN")]
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userService.GetUsers();
        if (users.Any())
        {
            return Ok(users);  // 200 OK, return the list of users
        }
        return NotFound("No users found.");  // 404 Not Found if no users
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDto">An object containing the user's registration details.</param>
    /// <returns>Returns a 201 Created status if the user is successfully registered, or a 400 or 409 status if validation fails.</returns>
    [Authorize(Policy = "USER")]
    [HttpPost]
    public IActionResult AddUser(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);  // 400 Bad Request, invalid input
        }

        var registerInfo = _userMapper.MapToRegisterInfo(registerDto);

        try
        {
            var username = _userManager.RegisterUser(registerInfo);  // Get the generated username
            return CreatedAtAction(nameof(AddUser), new { username }, new { message = "User registered successfully", username });  // Return username in the response
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);  // 409 Conflict if user already exists
        }
    }


    /// <summary>
    /// Updates an existing user's details.
    /// </summary>
    /// <param name="username">The username of the user to be updated.</param>
    /// <param name="updateUserDto">An object containing the updated user details.</param>
    /// <returns>Returns a 200 OK status if the update is successful, or 400/404 statuses for errors.</returns>
    [Authorize(Policy = "USER")]  // Accessible to both Admin and User
    [HttpPut("{username}")]
    public IActionResult UpdateUser(string username, [FromBody] UpdateUserDto updateUserDto)
    {
        var user = _userService.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound("User not found.");  // 404 Not Found
        }
        // Ensure users can only update their own info, unless they're an Admin
        if (User.Identity.Name != username && !User.IsInRole("ADMIN"))
        {
            return StatusCode(403, value: "You cannot update another user's profile.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);  // 400 Bad Request, invalid input
        }

        var updatedUser = _userMapper.MapToUpdatedUser(updateUserDto, user);
        _userManager.UpdateUser(updatedUser);

        return Ok("User updated successfully.");  // 200 OK
    }

    /// <summary>
    /// Removes a user from the system.
    /// </summary>
    /// <param name="username">The username of the user to be removed.</param>
    /// <returns>Returns a 200 OK status if the user is successfully removed, or a 404 Not Found status if the user does not exist.</returns>
    [Authorize(Policy = "USER")]
    [HttpDelete("{username}")]
    public IActionResult RemoveUser(string username)
    {
        var user = _userService.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound("User not found.");  // 404 Not Found
        }
        // Ensure users can only remove their own info, unless they're an Admin
        if (User.Identity.Name != username && !User.IsInRole("ADMIN"))
        {
            return StatusCode(403, value: "You cannot delete another user's profile.");
        }

        _userService.RemoveUser(username);
        return Ok("User removed successfully.");  // 200 OK
    }
    
    /// <summary>
    /// Updates the role of a user (Admin access only).
    /// </summary>
    /// <param name="username">The username of the user whose role is being updated.</param>
    /// <param name="newRole">The new role to be assigned to the user.</param>
    /// <returns>Returns a 200 OK status if the role update is successful, or 404/400 if errors occur.</returns>
    [Authorize(Policy = "ADMIN")]  // Only Admins can change roles
    [HttpPut("{username}/role")]
    public IActionResult UpdateUserRole(string username, [FromBody] string newRole)
    {
        // Ensure the role being assigned is valid
        if (!Enum.TryParse(typeof(UserRole), newRole, true, out var role))
        {
            return BadRequest("Invalid role.");
        }

        // Fetch the user
        var user = _userService.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Update the user's role
        user.Role = (UserRole)role;
        _userManager.UpdateUser(user);

        return Ok($"User {username}'s role updated to {newRole}.");
    }
}
