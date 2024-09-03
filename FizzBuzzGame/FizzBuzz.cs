namespace FizzBuzz
{
    /// <summary>
    /// Represents a FizzBuzz guess and its corresponding points.
    /// </summary>
    public class FizzBuzz
    {
        /// <summary>
        /// Gets or sets the points for the FizzBuzz guess.
        /// </summary>
        public int Point { get; private set; }

        /// <summary>
        /// Gets or sets the FizzBuzz guess type (Number, Fizz, Buzz, FizzBuzz).
        /// </summary>
        public FizzBuzzGuess Guess { get; private set; }

        /// <summary>
        /// Factory method to create a <see cref="FizzBuzz"/> instance based on a value.
        /// </summary>
        /// <param name="value">The integer value to evaluate.</param>
        /// <returns>A new instance of <see cref="FizzBuzz"/>.</returns>
        public static FizzBuzz Create(int value)
        {
            var fizzBuzz = new FizzBuzz();
            fizzBuzz.Guess = DetermineGuess(value);
            fizzBuzz.Point = CalculatePoints(fizzBuzz.Guess);
            return fizzBuzz;
        }

        /// <summary>
        /// Determines the FizzBuzz guess type for a given value.
        /// </summary>
        /// <param name="value">The integer value to evaluate.</param>
        /// <returns>The determined <see cref="FizzBuzzGuess"/>.</returns>
        private static FizzBuzzGuess DetermineGuess(int value)
        {
            if (value % 3 == 0 && value % 5 == 0)
                return FizzBuzzGuess.FIZZBUZZ;
            else if (value % 3 == 0)
                return FizzBuzzGuess.FIZZ;
            else if (value % 5 == 0)
                return FizzBuzzGuess.BUZZ;
            else
                return FizzBuzzGuess.NUMBER;
        }

        /// <summary>
        /// Calculates points based on the FizzBuzz guess type.
        /// </summary>
        /// <param name="guess">The <see cref="FizzBuzzGuess"/> type.</param>
        /// <returns>The points associated with the guess type.</returns>
        private static int CalculatePoints(FizzBuzzGuess guess)
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

    /// <summary>
    /// Represents the different types of FizzBuzz guesses.
    /// </summary>
    public enum FizzBuzzGuess
    {
        NUMBER,
        FIZZ,
        BUZZ,
        FIZZBUZZ
    }
}
