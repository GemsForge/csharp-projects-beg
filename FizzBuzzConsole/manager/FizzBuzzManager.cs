using FizzBuzzConsole.model;

namespace FizzBuzzConsole.manager
{
    /// <summary>
    /// Provides core logic for FizzBuzz operations, such as determining guesses and calculating points.
    /// </summary>
    public class FizzBuzzManager : IFizzBuzzManager
    {
        /// <summary>
        /// Determines the FizzBuzz guess type for a given value.
        /// </summary>
        /// <param name="value">The integer value to evaluate.</param>
        /// <returns>The determined <see cref="FizzBuzzGuess"/>.</returns>
        public FizzBuzzGuess DetermineGuess(int value)
        {
            if (value % 3 == 0 && value % 5 == 0)
                return FizzBuzzGuess.FIZZBUZZ;
            else if (value % 3 == 0)
                return FizzBuzzGuess.FIZZ;
            else if (value % 5 == 0)
                return FizzBuzzGuess.BUZZ;
            else
                return FizzBuzzGuess.NONE;
        }

        /// <summary>
        /// Calculates points based on the FizzBuzz guess type.
        /// </summary>
        /// <param name="guess">The <see cref="FizzBuzzGuess"/> type.</param>
        /// <returns>The points associated with the guess type.</returns>
        public int CalculatePoints(FizzBuzzGuess guess)
        {
            return guess switch
            {
                FizzBuzzGuess.FIZZBUZZ => 10,
                FizzBuzzGuess.FIZZ => 5,
                FizzBuzzGuess.BUZZ => 5,
                _ => 1,
            };
        }
    }
}
