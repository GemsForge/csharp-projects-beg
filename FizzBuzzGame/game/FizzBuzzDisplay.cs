namespace FizzBuzzConsole.game
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
            Console.WriteLine("   - Otherwise, no points are earned.");
            Console.WriteLine("Try to get the highest score possible!\n");
        }

        /// <summary>
        /// Displays the results of the FizzBuzz game round.
        /// </summary>
        public void DisplayResults(int fizzes, int buzzes, int fizzBuzzes)
        {
            Console.WriteLine($"Results: Fizz: {fizzes}, Buzz: {buzzes}, FizzBuzz: {fizzBuzzes}");
        }

        /// <summary>
        /// Asks the user if they want to play again.
        /// </summary>
        /// <returns>True if the user wants to play again; otherwise, false.</returns>
        public bool AskToPlayAgain()
        {
            Console.Write("Do you want to play again? (y/n): ");
            while (true)
            {
                string? answer = Console.ReadLine();
                if (answer is null)
                {
                    Console.WriteLine("Invalid input, try again.");
                    return false; // or handle accordingly
                }

                switch (answer.ToLower()) // Convert to lowercase to handle 'Y' and 'N' variations
                {
                    case "y":
                        return true; // Corrected to return `true`
                    case "n":
                        return false;
                    default:
                        Console.WriteLine("Invalid input, try again.");
                        break;
                }
            }


        }

        /// <summary>
        /// Displays the current cumulative score to the user.
        /// </summary>
        /// <param name="score">The user's current cumulative score.</param>
        public void DisplayScore(int score)
        {
            Console.WriteLine($"\nYour current score is: {score} points.\n");
        }

        /// <summary>
        /// Displays the final score after the user finishes playing.
        /// </summary>
        /// <param name="score">The user's final score.</param>
        public void DisplayFinalScore(int score)
        {
            Console.WriteLine($"\nThank you for playing! Your final score is: {score} points.");
        }

        /// <summary>
        /// Prompts the user to enter a list of validated inputs.
        /// </summary>
        /// <param name="count">The number of inputs to request from the user.</param>
        /// <returns>A list of validated integers entered by the user.</returns>
        public List<int> GetValidatedInputs(int count)
        {
            List<int> inputs = new();
            Console.WriteLine("Input a number between 1 and 100");
            while (inputs.Count < count)
            {
                Console.Write($"Enter number {inputs.Count + 1}: ");
                if (int.TryParse(Console.ReadLine(), out int input) && input >= 1 && input <= 100)
                {
                    inputs.Add(input);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 100.");
                }
            }
            return inputs;
        }
    }
}
