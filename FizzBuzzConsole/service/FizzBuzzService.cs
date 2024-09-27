using CommonLibrary.Data;
using FizzBuzzConsole.model;

namespace FizzBuzzConsole.service
{
    /// <summary>
    /// Provides game service logic for the FizzBuzz program.
    /// Points are awarded based on whether a number is "Fizz", "Buzz", or "FizzBuzz".
    /// </summary>
    public class FizzBuzzService : IFizzBuzzService
    {
        private readonly IGenericRepository<FizzBuzzGamePlay> _fbRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzService"/> class.
        /// </summary>
        public FizzBuzzService(IGenericRepository<FizzBuzzGamePlay> fbRepo)
        {
            _fbRepo = fbRepo;
        }

        /// <summary>
        /// Creates a new gameplay session and saves the provided list of integer values as FizzBuzz guesses.
        /// </summary>
        /// <param name="player">The player (User ID or Username) for the session.</param>
        /// <param name="values">The list of integer values to evaluate.</param>
        public void SaveGamePlay(string player, List<int> values)
        {
            var gamePlays = _fbRepo.GetAll().ToList(); // Load existing data each time
            var gamePlayId = gamePlays.Any() ? gamePlays.Max(gp => gp.GamePlayId) + 1 : 1;

            FizzBuzzGamePlay newGamePlay = new FizzBuzzGamePlay
            {
                GamePlayId = gamePlayId,
                Player = int.Parse(player),
                Guesses = new Dictionary<int, FizzBuzzGuess>(),
                TotalPoints = 0
            };

            foreach (var value in values)
            {
                var guess = DetermineGuess(value);
                newGamePlay.AddGuess(value, guess);
            }

            _fbRepo.Add(newGamePlay); // Directly save using repository
        }

        /// <summary>
        /// Clears the gameplay results for a specific session.
        /// </summary>
        /// <param name="gamePlayId">The ID of the gameplay session to clear.</param>
        public void ClearGamePlay(int gamePlayId)
        {
            _fbRepo.Remove(gamePlayId); // Directly remove using repository
        }

        /// <summary>
        /// Retrieves all saved gameplay sessions for a specific player.
        /// </summary>
        /// <param name="player">The player's ID.</param>
        /// <returns>A list of FizzBuzzGamePlay objects for the player.</returns>
        public IEnumerable<FizzBuzzGamePlay> GetGamePlaysForPlayer(int player)
        {
            return _fbRepo.GetAll().Where(gp => gp.Player == player);
        }

        /// <summary>
        /// Counts the occurrences of Fizz, Buzz, and FizzBuzz based on the guesses for a specific gameplay session.
        /// </summary>
        /// <param name="gamePlayId">The ID of the gameplay session.</param>
        /// <returns>A tuple containing the counts of Fizz, Buzz, and FizzBuzz.</returns>
        public (int fizzes, int buzzes, int fizzBuzzes) CountFizzBuzzes(int gamePlayId)
        {
            var gamePlay = _fbRepo.GetById(gamePlayId);
            if (gamePlay == null)
            {
                return (0, 0, 0);
            }

            return (
                gamePlay.Guesses.Values.Count(x => x == FizzBuzzGuess.FIZZ),
                gamePlay.Guesses.Values.Count(x => x == FizzBuzzGuess.BUZZ),
                gamePlay.Guesses.Values.Count(x => x == FizzBuzzGuess.FIZZBUZZ)
            );
        }

        /// <summary>
        /// Calculates the total points for a specific gameplay session.
        /// </summary>
        /// <param name="gamePlayId">The ID of the gameplay session.</param>
        /// <returns>The total calculated points.</returns>
        public int TallyPoints(int gamePlayId)
        {
            var gamePlay = _fbRepo.GetById(gamePlayId);
            return gamePlay?.TotalPoints ?? 0;
        }

        /// <summary>
        /// Determines the FizzBuzz guess type for a given value.
        /// </summary>
        /// <param name="value">The integer value to evaluate.</param>
        /// <returns>The determined FizzBuzzGuess.</returns>
        private FizzBuzzGuess DetermineGuess(int value)
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
    }
}
