using FizzBuzzConsole.service;

namespace FizzBuzzConsole.game
{
    /// <summary>
    /// Handles the core game logic for FizzBuzz.
    /// </summary>
    public class FizzBuzzArcade
    {
        private readonly IFizzBuzzService _fbService;
        private readonly IFizzBuzzDisplay _display;
        private int _score;  // Variable to track the user's cumulative score
        private string _playerId;  // Variable to store the current player's ID
        private int _currentGamePlayId;  // Variable to track the current GamePlayId


        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzArcade"/> class.
        /// </summary>
        public FizzBuzzArcade(IFizzBuzzService fizzBuzzService, IFizzBuzzDisplay display, string playerId)
        {
            _fbService = fizzBuzzService;
            _display = display;
            _playerId = playerId;  // Initialize with the player's ID
            _score = 0;  // Initialize score to 0
        }

        /// <summary>
        /// Starts the FizzBuzz game.
        /// </summary>
        public void StartGame()
        {
            bool playAgain = true;
            _display.DisplayGameRules();

            while (playAgain)
            {
                // Create a new GamePlayId for each session
                _currentGamePlayId = GenerateGamePlayId();

                List<int> inputs = _display.GetValidatedInputs(5);
                _fbService.SaveGamePlay(_playerId, inputs);  // Save with player ID and inputs

                var (fizzes, buzzes, fizzBuzzes) = _fbService.CountFizzBuzzes(_currentGamePlayId);
                _display.DisplayResults(fizzes, buzzes, fizzBuzzes);

                UpdateScore(fizzes, buzzes, fizzBuzzes);  // Update score based on game results
                _display.DisplayScore(_score);  // Display the current cumulative score

                // Ask if the user wants to play again
                playAgain = _display.AskToPlayAgain();
                if (playAgain)
                {
                    ResetGame();  // Reset game-specific state but keep the cumulative score
                }
            }

            _display.DisplayFinalScore(_score);  // Display the final cumulative score after all games
        }

        /// <summary>
        /// Resets the game-specific state for a new session.
        /// </summary>
        private void ResetGame()
        {
            _fbService.ClearGamePlay(_currentGamePlayId);  // Clear previous game results for this session
        }

        /// <summary>
        /// Updates the user's score based on the results of the game.
        /// </summary>
        /// <param name="fizzes">The count of 'Fizz' results.</param>
        /// <param name="buzzes">The count of 'Buzz' results.</param>
        /// <param name="fizzBuzzes">The count of 'FizzBuzz' results.</param>
        private void UpdateScore(int fizzes, int buzzes, int fizzBuzzes)
        {
            // Example scoring logic: 1 point for each Fizz, 2 points for each Buzz, 3 points for each FizzBuzz
            _score += fizzes * 1 + buzzes * 2 + fizzBuzzes * 3;
        }

        /// <summary>
        /// Generates a unique GamePlayId by getting the next available ID from the service.
        /// </summary>
        /// <returns>The generated GamePlayId.</returns>
        private int GenerateGamePlayId()
        {
            // This method assumes that the service can generate a new sequential GamePlayId
            var gamePlays = _fbService.GetGamePlaysForPlayer(_playerId);
            return gamePlays.Any() ? gamePlays.Max(gp => gp.GamePlayId) + 1 : 1;
        }
    }
}
