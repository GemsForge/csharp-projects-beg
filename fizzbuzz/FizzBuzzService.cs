using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fizzbuzz
{
    //Game: Choose 3 numbers.
    //Normal numbers = 1pt.
    //Fizz & Buzz = 5pts
    //FizzBuzz = 10 pts
    public class FizzBuzzService
    {
        private readonly int _value;
        private List<int> _values;

        public FizzBuzzService()
        {
            _values = new();
        }

        public bool IsFizz(int value)
        {
            var result = value % 3 == 0;

            return result;
        }
        public bool IsBuzz(int value)
        {
            return value % 5 == 0;
        }
        public bool IsFizzBuzz(int value)
        {
            return value % 3 == 0 && value % 5 == 0;
        }

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