namespace FizzBuzzConsole.model
{
    /// <summary>
    /// Represents a gameplay session for the FizzBuzz game, storing guesses and points for a specific player.
    /// </summary>
    public class FizzBuzzGamePlay
    {
        /// <summary>
        /// Gets or sets the unique ID for each gameplay session.
        /// </summary>
        /// <example>1</example>
        public int GamePlayId { get; set; }

        /// <summary>
        /// Gets or sets the player identifier (could be User ID or Username).
        /// </summary>
        /// <example>user123</example>
        public string Player { get; set; }

        /// <summary>
        /// Gets or sets the key-value pairs of guesses made during the gameplay.
        /// The key represents the number, and the value represents the FizzBuzz guess (e.g., "FIZZ", "BUZZ", "FIZZBUZZ", or "NONE").
        /// </summary>
        /// <example>{ "3": "FIZZ", "5": "BUZZ", "15": "FIZZBUZZ", "7": "NONE" }</example>
        public Dictionary<int, FizzBuzzGuess> Guesses { get; set; } = new();

        /// <summary>
        /// Gets or sets the total points scored in the gameplay.
        /// </summary>
        /// <example>21</example>
        public int TotalPoints { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzGamePlay"/> class with the given gameplay ID and player.
        /// </summary>
        /// <param name="gamePlayId">The unique ID for the gameplay session.</param>
        /// <param name="player">The identifier of the player (User ID or Username).</param>
        /// <example>new FizzBuzzGamePlay(1, "user123")</example>
        public FizzBuzzGamePlay(int gamePlayId, string player)
        {
            GamePlayId = gamePlayId;
            Player = player;
        }

        /// <summary>
        /// Adds a guess for a specific number and calculates the points based on the guess.
        /// </summary>
        /// <param name="number">The number being evaluated in the FizzBuzz game.</param>
        /// <param name="guess">The FizzBuzz guess for the number (e.g., FIZZ, BUZZ, FIZZBUZZ, or NONE).</param>
        /// <example>AddGuess(3, FizzBuzzGuess.FIZZ)</example>
        public void AddGuess(int number, FizzBuzzGuess guess)
        {
            Guesses[number] = guess;
            TotalPoints += CalculatePoints(guess);
        }

        /// <summary>
        /// Calculates the points based on the FizzBuzz guess type.
        /// </summary>
        /// <param name="guess">The FizzBuzz guess (FIZZ, BUZZ, FIZZBUZZ, or NONE).</param>
        /// <returns>The points awarded based on the guess.</returns>
        /// <example>
        /// FIZZBUZZ: 10, 
        /// FIZZ: 5, 
        /// BUZZ: 5, 
        /// NONE: 1
        /// </example>
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
}
