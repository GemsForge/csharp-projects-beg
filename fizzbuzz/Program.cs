using FizzBuzz;

public class Program
{
    private static void Main(string[] args)
    {
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
