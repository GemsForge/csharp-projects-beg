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

        public int TalleyPoints()
        {
            int total = 0;
            foreach(int value  in _values)
            {
                if (IsBuzz(value) == true)
                {
                    total += 5;
                    continue;
                }
                if (IsFizzBuzz(value) == true)
                {
                    total += 5;
                    continue;
                }
                if (IsFizzBuzz(_value) == true)
                {
                    total+= 10;
                }
                else
                {
                    total++;
                }

            }
            return total;

        }
    }
}
