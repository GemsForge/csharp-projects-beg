using FizzBuzz;
using TaskTracker.BrandLogo;

public class Program
{
    private static void Main(string[] args)
    {
        Logo.DisplayLogo();
        // Display game rules to the console
        DisplayGameRules();

        FizzBuzzService fbService = new FizzBuzzService();

        // Collect inputs
        List<int> inputs = GetValidatedInputs(5);

        // Save inputs and display results
        fbService.SaveValueList(inputs);

        // Count occurrences and tally points
        var (fizzes, buzzes, fizzBuzzes) = CountFizzBuzzes(fbService);

        // Display the results
        Console.WriteLine($"Total Points: {fbService.TallyPoints()} (Fizz {fizzes}x | Buzz {buzzes}x | FizzBuzz {fizzBuzzes}x)");
    }

    /// <summary>
    /// Displays the rules of the FizzBuzz game to the console.
    /// </summary>
    private static void DisplayGameRules()
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
    private static List<int> GetValidatedInputs(int numberOfInputs)
    {
        List<int> inputs = new List<int>();
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
    /// Counts the occurrences of Fizz, Buzz, and FizzBuzz based on the saved FizzBuzz values.
    /// </summary>
    /// <param name="fbService">The FizzBuzz service that holds the saved values.</param>
    /// <returns>A tuple containing the counts of Fizz, Buzz, and FizzBuzz.</returns>
    private static (int fizzes, int buzzes, int fizzBuzzes) CountFizzBuzzes(FizzBuzzService fbService)
    {
        int fizzes = 0;
        int buzzes = 0;
        int fizzBuzzes = 0;

        foreach (var fizzBuzz in fbService.GetSavedValues())
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
