using FizzBuzzConsole.data;
using FizzBuzzConsole.model;
using System.Collections.Generic;
using System.Linq;

namespace FizzBuzzConsole.service
{
    /// <summary>
    /// Provides game service logic for the FizzBuzz program.
    /// Points are awarded based on whether a number is "Fizz", "Buzz", or "FizzBuzz".
    /// </summary>
    public class FizzBuzzService : IFizzBuzzService
    {
        private readonly IFizzBuzzRepository _repo;
        private readonly List<FizzBuzzGamePlay> _gamePlays;  // Use the FizzBuzzGamePlay model to store sessions

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzService"/> class.
        /// </summary>
        public FizzBuzzService(IFizzBuzzRepository repo)
        {
            _repo = repo;
            _gamePlays = _repo.LoadResults();  // Load existing results when initializing the service
        }

        /// <summary>
        /// Creates a new gameplay session and saves the provided list of integer values as FizzBuzz guesses.
        /// </summary>
        /// <param name="player">The player (User ID or Username) for the session.</param>
        /// <param name="values">The list of integer values to evaluate.</param>
        public void SaveGamePlay(string player, List<int> values)
        {
            // Generate the next sequential GamePlayId
            var gamePlayId = _gamePlays.Count != 0 ? _gamePlays.Max(gp => gp.GamePlayId) + 1 : 1;

            FizzBuzzGamePlay newGamePlay = new FizzBuzzGamePlay
            {
                GamePlayId = gamePlayId,
                Player = int.Parse(player),
                Guesses = [],
                TotalPoints = 0
            };

            foreach (var value in values)
            {
                var guess = DetermineGuess(value);  // Determine the FizzBuzz guess
                newGamePlay.AddGuess(value, guess);  // Add guess and update total points
            }

            _gamePlays.Add(newGamePlay);  // Add the new gameplay session to the list
            _repo.SaveResults(_gamePlays);  // Persist the updated list to the JSON file
        }

        /// <summary>
        /// Clears the gameplay results for a specific session.
        /// </summary>
        /// <param name="gamePlayId">The ID of the gameplay session to clear.</param>
        public void ClearGamePlay(int gamePlayId)
        {
            var gamePlay = _gamePlays.FirstOrDefault(gp => gp.GamePlayId == gamePlayId);
            if (gamePlay != null)
            {
                _gamePlays.Remove(gamePlay);  // Remove the specific gameplay session
                _repo.SaveResults(_gamePlays);  // Persist the updated list
            }
        }

        /// <summary>
        /// Counts the occurrences of Fizz, Buzz, and FizzBuzz based on the guesses for a specific gameplay session.
        /// </summary>
        /// <param name="gamePlayId">The ID of the gameplay session.</param>
        /// <returns>A tuple containing the counts of Fizz, Buzz, and FizzBuzz.</returns>
        public (int fizzes, int buzzes, int fizzBuzzes) CountFizzBuzzes(int gamePlayId)
        {
            var gamePlay = _gamePlays.FirstOrDefault(gp => gp.GamePlayId == gamePlayId);
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
        /// Retrieves all saved gameplay sessions for a specific player.
        /// </summary>
        /// <param name="player">The player's ID or username.</param>
        /// <returns>A list of FizzBuzzGamePlay objects for the player.</returns>
        public IEnumerable<FizzBuzzGamePlay> GetGamePlaysForPlayer(int player)
        {
            return _gamePlays.Where(gp => gp.Player == player);
        }

        /// <summary>
        /// Calculates the total points for a specific gameplay session.
        /// </summary>
        /// <param name="gamePlayId">The ID of the gameplay session.</param>
        /// <returns>The total calculated points.</returns>
        public int TallyPoints(int gamePlayId)
        {
            var gamePlay = _gamePlays.FirstOrDefault(gp => gp.GamePlayId == gamePlayId);
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
