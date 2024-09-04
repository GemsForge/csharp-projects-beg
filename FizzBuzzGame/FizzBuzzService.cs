using FizzBuzz.service;

namespace FizzBuzz
{
    /// <summary>
    /// Provides game service logic for the FizzBuzz program.
    /// Points are awarded based on whether a number is "Fizz", "Buzz", or "FizzBuzz".
    /// Normal numbers are worth 1 point, "Fizz" or "Buzz" are worth 5 points, and "FizzBuzz" is worth 10 points.
    /// </summary>
    public class FizzBuzzService : IFizzBuzzService
    {
        private readonly List<FizzBuzz> _values;  // Use the FizzBuzz model to store values and their guesses

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzService"/> class.
        /// </summary>
        public FizzBuzzService()
        {
            _values = new List<FizzBuzz>();
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
