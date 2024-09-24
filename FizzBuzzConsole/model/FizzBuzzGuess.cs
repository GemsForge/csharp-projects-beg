namespace FizzBuzzConsole.model
{
    /// <summary>
    /// Represents the different types of FizzBuzz guesses.
    /// </summary>
    public enum FizzBuzzGuess
    {
        /// <summary>
        /// Represents a standard number that is neither divisible by 3 nor 5.
        /// </summary>
        /// <example>NONE</example>
        NONE,

        /// <summary>
        /// Represents a number divisible by 3 (Fizz).
        /// </summary>
        /// <example>FIZZ</example>
        FIZZ,

        /// <summary>
        /// Represents a number divisible by 5 (Buzz).
        /// </summary>
        /// <example>BUZZ</example>
        BUZZ,

        /// <summary>
        /// Represents a number divisible by both 3 and 5 (FizzBuzz).
        /// </summary>
        /// <example>FIZZBUZZ</example>
        FIZZBUZZ
    }
}
