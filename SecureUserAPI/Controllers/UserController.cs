using Microsoft.AspNetCore.Mvc;
using SecureUserAPI.DTO;

using SecureUserConsole.service;

/// <summary>
/// Handles operations related to user management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserManager _userManager;
    private readonly IUserService _userService;
    private readonly IUserMapper _userMapper;

    public UserController(IUserManager userManager, IUserService userService, IUserMapper userMapper)
    {
        _userManager = userManager;
        _userService = userService;
        _userMapper = userMapper;
    }

    /// <summary>
    /// Retrieves a list of all registered users.
    /// </summary>
    /// <returns>Returns a list of users or a 404 status if no users are found.</returns>
    [HttpGet]
    public IActionResult ListUsers()
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
    [HttpPut("{username}")]
    public IActionResult UpdateUser(string username, UpdateUserDto updateUserDto)
    {
        var user = _userService.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound("User not found.");  // 404 Not Found
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
    [HttpDelete("{username}")]
    public IActionResult RemoveUser(string username)
    {
        var user = _userService.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound("User not found.");  // 404 Not Found
        }

        _userService.RemoveUser(username);
        return Ok("User removed successfully.");  // 200 OK
    }
}
