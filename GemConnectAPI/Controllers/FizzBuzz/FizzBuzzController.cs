
using FizzBuzzConsole.service;
using GemConnectAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace GemConnectAPI.Controllers.FizzBuzz
{
    //[Authorize(Policy = "USER")]
    [Route("api/[controller]")]
    [ApiController]
    public class FizzBuzzController : ControllerBase
    {
        private readonly IFizzBuzzService _fbService;

        /// <summary>
        /// Initializes a new instances of the <see cref="FizzBuzzController"/>
        /// </summary>
        /// <param name="_fbService">The service for handling FizzBuzz game logic</returns>
        public FizzBuzzController(IFizzBuzzService fbService)
        {
            _fbService = fbService;
        }

        /// <summary>
        /// Saves a list of values to calculate FizzBuzz results for the logged-in user.
        /// </summary>
        /// <param name="fbDto">The list of integer values to process.</param>
        /// <remarks>
        /// Example request:
        ///
        ///     POST /api/FizzBuzz/values
        ///     [1, 3, 5, 15, 7]
        ///
        /// </remarks>
        [HttpPost("values")]
        public IActionResult SaveValues([FromBody] FizzBuzzDto fbDto)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the logged-in user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            // Save the gameplay for the logged-in user with sequential GamePlayId
            _fbService.SaveGamePlay(userId, fbDto.Values);

            return CreatedAtAction(nameof(GetSavedValues), new { }, _fbService.GetGamePlaysForPlayer(userId));
        }

        /// <summary>
        /// Clears the previous gameplay results for the logged-in user.
        /// </summary>
        /// <remarks>
        /// Example request:
        ///
        ///     DELETE /api/FizzBuzz/clear/{gamePlayId}
        ///
        /// </remarks>
        [HttpDelete("clear/{gamePlayId}")]
        public IActionResult ClearResults(int gamePlayId)
        {
            // Get the logged-in user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            _fbService.ClearGamePlay(gamePlayId);
            return NoContent();
        }

        /// <summary>
        /// Gets the count of Fizz, Buzz, and FizzBuzz occurrences for the logged-in user.
        /// </summary>
        /// <returns>A tuple containing the counts of Fizz, Buzz, and FizzBuzz.</returns>
        /// <remarks>
        /// Example request:
        ///
        ///     GET /api/FizzBuzz/count/{gamePlayId}
        ///
        /// </remarks>
        [HttpGet("count/{gamePlayId}")]
        public IActionResult GetFizzBuzzCount(int gamePlayId)
        {
            var result = _fbService.CountFizzBuzzes(gamePlayId);
            return Ok(new { Fizz = result.fizzes, Buzz = result.buzzes, FizzBuzz = result.fizzBuzzes });
        }
        /// <summary>
        /// Gets the total points based on the saved FizzBuzz guesses for a specific gameplay session.
        /// </summary>
        /// <param name="gamePlayId">The ID of the gameplay session.</param>
        /// <returns>The total calculated points.</returns>
        /// <remarks>
        /// Example request:
        ///
        ///     GET /api/FizzBuzz/points/{gamePlayId}
        ///
        /// </remarks>
        [HttpGet("points/{gamePlayId}")]
        public IActionResult GetTotalPoints(int gamePlayId)
        {
            int totalPoints = _fbService.TallyPoints(gamePlayId);
            return Ok(new { TotalPoints = totalPoints });
        }
                /// <summary>
        /// Retrieves all saved FizzBuzz values and their results.
        /// </summary>
        /// <returns>A list of saved FizzBuzz values and their results.</returns>
        /// <remarks>
        /// Example request:
        ///
        ///     GET /api/FizzBuzz/values
        ///
        /// </remarks>
        [HttpGet("values")]
        public IActionResult GetSavedValues()
        {
            //Get the logged-in user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            var savedValues = _fbService.GetGamePlaysForPlayer(userId);
            return Ok(savedValues);
        }
    }
}
