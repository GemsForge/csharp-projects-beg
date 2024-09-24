
using FizzBuzzConsole.service;
using GemConnectAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
        /// Saves a list of values to calculate FizzBuzz results.
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

            _fbService.SaveValueList(fbDto.Values);

            // Return a 201 Created response with the saved values in the body
            return CreatedAtAction(nameof(GetSavedValues), _fbService.GetSavedValues());
        }

        /// <summary>
        /// Clears the previous results to reset the game state.
        /// </summary>
        /// <remarks>
        /// Example request:
        ///
        ///     DELETE /api/FizzBuzz/clear
        ///
        /// </remarks>
        [HttpDelete("clear")]
        public IActionResult ClearResults()
        {
            _fbService.ClearPreviousResults();
            return NoContent();
        }

        /// <summary>
        /// Gets the count of Fizz, Buzz, and FizzBuzz occurrences.
        /// </summary>
        /// <returns>A tuple containing the counts of Fizz, Buzz, and FizzBuzz.</returns>
        /// <remarks>
        /// Example request:
        ///
        ///     GET /api/FizzBuzz/count
        ///
        /// </remarks>
        [HttpGet("count")]
        public IActionResult GetFizzBuzzCount()
        {
            var result = _fbService.CountFizzBuzzes();
            return Ok(new { Fizz = result.fizzes, Buzz = result.buzzes, FizzBuzz = result.fizzBuzzes });
        }

        /// <summary>
        /// Gets the total points based on the saved FizzBuzz guesses.
        /// </summary>
        /// <returns>The total calculated points.</returns>
        /// <remarks>
        /// Example request:
        ///
        ///     GET /api/FizzBuzz/points
        ///
        /// </remarks>
        [HttpGet("points")]
        public IActionResult GetTotalPoints()
        {
            int totalPoints = _fbService.TallyPoints();
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
            var savedValues = _fbService.GetSavedValues();
            return Ok(savedValues);
        }
    }
}
