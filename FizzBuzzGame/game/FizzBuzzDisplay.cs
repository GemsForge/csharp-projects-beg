namespace FizzBuzzGame.game
{
    /// <summary>
    /// Handles the display and user interaction for the FizzBuzz game.
    /// </summary>
    public class FizzBuzzDisplay : IFizzBuzzDisplay
    {
        /// <summary>
        /// Displays the rules of the FizzBuzz game to the console.
        /// </summary>
        public void DisplayGameRules()
        {
            Console.WriteLine("Welcome to the FizzBuzz Game!");
            Console.WriteLine("Here are the rules:");
            Console.WriteLine("1. You will enter 5 numbers between 1 and 100.");
            Console.WriteLine("2. For each number:");
            Console.WriteLine("   - If the number is divisible by 3 and 5, it's a 'FizzBuzz'. You earn 10 points.");
            Console.WriteLine("   - If the number is divisible by 3 but not by 5, it's a 'Fizz'. You earn 5 points.");
            Console.WriteLine("   - If the number is divisible by 5 but not by 3, it's a 'Buzz'. You earn 5 points.");
            Console.WriteLine("   - If the number is not divisible by either 3 or 5, it's a 'Number'. You earn 1 point.");
            Console.WriteLine("3. The goal is to accumulate as many points as possible!");
            Console.WriteLine("Let's get started!\n");
        }

        /// <summary>
        /// Gets a list of validated integer inputs from the user.
        /// </summary>
        /// <param name="numberOfInputs">The number of inputs to collect.</param>
        /// <returns>A list of validated integers.</returns>
        public List<int> GetValidatedInputs(int numberOfInputs)
        {
            List<int> inputs = new();
            Console.WriteLine($"Enter {numberOfInputs} numbers between 1 and 100:");

            for (int i = 1; i <= numberOfInputs; i++)
            {
                while (true) 
                {
                    Console.Write($"Input {i}: ");
                    if (!int.TryParse(Console.ReadLine(), out int input) || input < 1 || input > 100)
                    {
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 100.");
                        continue;
                    }
                    inputs.Add(input);
                    break;
                }
            }

            return inputs;
        }

        /// <summary>
        /// Displays the results of the FizzBuzz game.
        /// </summary>
        /// <param name="totalPoints">The total points scored.</param>
        /// <param name="fizzes">The number of "Fizz" occurrences.</param>
        /// <param name="buzzes">The number of "Buzz" occurrences.</param>
        /// <param name="fizzBuzzes">The number of "FizzBuzz" occurrences.</param>
        public void DisplayResults(int totalPoints, int fizzes, int buzzes, int fizzBuzzes)
        {
            Console.WriteLine($"Total Points: {totalPoints} (Fizz {fizzes}x | Buzz {buzzes}x | FizzBuzz {fizzBuzzes}x)");
        }
    }
}
