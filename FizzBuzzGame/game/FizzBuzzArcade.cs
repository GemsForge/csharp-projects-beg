using FizzBuzzGame.service;
using FizzBuzzGame.model;

namespace FizzBuzzGame.game
{
    /// <summary>
    /// Handles the core game logic for FizzBuzz.
    /// </summary>
    public class FizzBuzzArcade
    {
        private readonly IFizzBuzzService _fbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzArcade"/> class.
        /// </summary>
        public FizzBuzzArcade(IFizzBuzzService fizzBuzzService)
        {
            _fbService = fizzBuzzService;
        }

        /// <summary>
        /// Starts the FizzBuzz game.
        /// </summary>
        public void StartGame()
        {
            List<int> inputs = FizzBuzzDisplay.GetValidatedInputs(5);
            _fbService.SaveValueList(inputs);

            var (fizzes, buzzes, fizzBuzzes) = CountFizzBuzzes();
            FizzBuzzDisplay.DisplayResults(_fbService.TallyPoints(), fizzes, buzzes, fizzBuzzes);
        }

        /// <summary>
        /// Counts the occurrences of Fizz, Buzz, and FizzBuzz based on the saved FizzBuzz values.
        /// </summary>
        /// <returns>A tuple containing the counts of Fizz, Buzz, and FizzBuzz.</returns>
        private (int fizzes, int buzzes, int fizzBuzzes) CountFizzBuzzes()
        {
            int fizzes = 0;
            int buzzes = 0;
            int fizzBuzzes = 0;

            foreach (var fizzBuzz in _fbService.GetSavedValues())
            {
                switch (fizzBuzz.Guess)
                {
                    case FizzBuzzGuess.FIZZ:
                        fizzes++;
                        break;
                    case FizzBuzzGuess.BUZZ:
                        buzzes++;
                        break;
                    case FizzBuzzGuess.FIZZBUZZ:
                        fizzBuzzes++;
                        break;
                }
            }

            return (fizzes, buzzes, fizzBuzzes);
        }
    }
}
