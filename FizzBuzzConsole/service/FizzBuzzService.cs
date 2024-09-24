using FizzBuzzConsole.data;
using FizzBuzzConsole.model;

namespace FizzBuzzConsole.service
{
    /// <summary>
    /// Provides game service logic for the FizzBuzz program.
    /// Points are awarded based on whether a number is "Fizz", "Buzz", or "FizzBuzz".
    /// Normal numbers are worth 1 point, "Fizz" or "Buzz" are worth 5 points, and "FizzBuzz" is worth 10 points.
    /// </summary>
    public class FizzBuzzService : IFizzBuzzService
    {
        private IFizzBuzzRepository _repo;
        private readonly List<FizzBuzz> _values;  // Use the FizzBuzz model to store values and their guesses

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzService"/> class.
        /// </summary>
        public FizzBuzzService(IFizzBuzzRepository repo)
        {
            _repo = repo;
            _values = _repo.LoadResults(); //Load existing results when initializing the service
        }

        /// <summary>
        /// Saves the provided list of integer values as FizzBuzz guesses for later use in calculating points.
        /// </summary>
        /// <param name="values">The list of integer values to save.</param>
        public void SaveValueList(List<int> values)
        {
            foreach (var value in values)
            {
                var fizzBuzz = FizzBuzz.Create(value);
                _values.Add(fizzBuzz);
            }
            _repo.SaveResults(_values);
        }
        /// <summary>
        /// Clears the previous results to reset the game state.
        /// </summary>
        public void ClearPreviousResults()
        {
            _values.Clear();  // Clear stored FizzBuzz values
            _repo.SaveResults(_values);
        }

        /// <summary>
        /// Counts the occurrences of Fizz, Buzz, and FizzBuzz based on the saved FizzBuzz values.
        /// </summary>
        /// <returns>A tuple containing the counts of Fizz, Buzz, and FizzBuzz.</returns>
        public (int fizzes, int buzzes, int fizzBuzzes) CountFizzBuzzes()
        {
            // Similar to before, counting FizzBuzzes
            return (
                _values.Count(x => x.Guess == FizzBuzzGuess.FIZZ),
                _values.Count(x => x.Guess == FizzBuzzGuess.BUZZ),
                _values.Count(x => x.Guess == FizzBuzzGuess.FIZZBUZZ)
            );
        }

        /// <summary>
        /// Calculates the total points based on the saved FizzBuzz guesses.
        /// </summary>
        /// <returns>The total calculated points.</returns>
        public int TallyPoints()
        {
            int total = 0;
            foreach (var item in _values)
            {
                total += item.Point;
            }
            return total;
        }

        public IEnumerable<FizzBuzz> GetSavedValues()
        {
            return _values;
        }
    }
}
