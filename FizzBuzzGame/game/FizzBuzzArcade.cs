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
        private readonly IFizzBuzzDisplay _display;

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzArcade"/> class.
        /// </summary>
        public FizzBuzzArcade(IFizzBuzzService fizzBuzzService, IFizzBuzzDisplay display)
        {
            _fbService = fizzBuzzService;
            _display = display;
        }

        /// <summary>
        /// Starts the FizzBuzz game.
        /// </summary>
        public void StartGame()
        {
            _display.DisplayGameRules();
            List<int> inputs = _display.GetValidatedInputs(5);
            _fbService.SaveValueList(inputs);

            var (fizzes, buzzes, fizzBuzzes) = CountFizzBuzzes();
            _display.DisplayResults(_fbService.TallyPoints(), fizzes, buzzes, fizzBuzzes);
            EndGame();
        }

        public void EndGame()
        {

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Play Again: y or n");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "y":
                        StartGame();
                        break;
                    case "n":
                        exit = true; break;
                    default:
                        Console.WriteLine("Invalid input, try again");
                        break;
                }
            }

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
