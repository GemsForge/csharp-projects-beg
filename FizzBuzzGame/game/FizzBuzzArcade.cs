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


        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzArcade"/> class.
        /// </summary>
        public FizzBuzzArcade(IFizzBuzzService fizzBuzzService, IFizzBuzzDisplay display)
        {
            _fbService = fizzBuzzService;
            _display = display;
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

                List<int> inputs = _display.GetValidatedInputs(5);
                _fbService.SaveValueList(inputs);

                var (fizzes, buzzes, fizzBuzzes) = _fbService.CountFizzBuzzes();
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
            _fbService.ClearPreviousResults();  // Clear previous game results

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


    }
}
