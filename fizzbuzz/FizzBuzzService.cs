using System;
using System.Collections.Generic;

namespace fizzbuzz
{
    /// <summary>
    /// Provides game service logic for the FizzBuzz program.
    /// Points are awarded based on whether a number is "Fizz", "Buzz", or "FizzBuzz".
    /// Normal numbers are worth 1 point, "Fizz" or "Buzz" are worth 5 points, and "FizzBuzz" is worth 10 points.
    /// </summary>
    public class FizzBuzzService
    {
        private readonly int _value;
        private List<int> _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzService"/> class.
        /// </summary>
        public FizzBuzzService()
        {
            _values = new List<int>();
        }

        /// <summary>
        /// Determines if the provided value is a "Fizz" (divisible by 3 but not by 5).
        /// </summary>
        /// <param name="value">The integer value to evaluate.</param>
        /// <returns><c>true</c> if the value is "Fizz"; otherwise, <c>false</c>.</returns>
        public bool IsFizz(int value) => value % 3 == 0 && value % 5 != 0;

        /// <summary>
        /// Determines if the provided value is a "Buzz" (divisible by 5 but not by 3).
        /// </summary>
        /// <param name="value">The integer value to evaluate.</param>
        /// <returns><c>true</c> if the value is "Buzz"; otherwise, <c>false</c>.</returns>
        public bool IsBuzz(int value) => value % 5 == 0 && value % 3 != 0;

        /// <summary>
        /// Determines if the provided value is a "FizzBuzz" (divisible by both 3 and 5).
        /// </summary>
        /// <param name="value">The integer value to evaluate.</param>
        /// <returns><c>true</c> if the value is "FizzBuzz"; otherwise, <c>false</c>.</returns>
        public bool IsFizzBuzz(int value) => value % 3 == 0 && value % 5 == 0;

        /// <summary>
        /// Saves the provided list of integer values for later use in calculating points.
        /// </summary>
        /// <param name="values">The list of integer values to save.</param>
        public void SaveValueList(List<int> values)
        {
            _values = values;
        }

        /// <summary>
        /// Calculates the total points based on the values provided in the collection.
        /// Points are awarded as follows:
        /// - 10 points if the value is considered "FizzBuzz".
        /// - 5 points if the value is considered "Fizz" (and not "FizzBuzz").
        /// - 5 points if the value is considered "Buzz" (and not "Fizz" or "FizzBuzz").
        /// - 1 point if the value is neither "Fizz", "Buzz", nor "FizzBuzz".
        /// </summary>
        /// <returns>The total calculated points.</returns>
        public int TalleyPoints()
        {
            int total = 0;
            foreach (int value in _values)
            {
                // Add 10 pts if FizzBuzz
                if (IsFizzBuzz(value))
                {
                    total += 10;
                }
                // Add 5 pts if Fizz (only if not FizzBuzz)
                else if (IsFizz(value))
                {
                    total += 5;
                }
                // Add 5 pts if Buzz (only if not Fizz or FizzBuzz)
                else if (IsBuzz(value))
                {
                    total += 5;
                }
                // Add 1 pt if Number (only if not Fizz, Buzz, or FizzBuzz)
                else
                {
                    total++;
                }
            }
            return total;
        }
    }
}
