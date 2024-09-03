// See https://aka.ms/new-console-template for more information


using FizzBuzz;

public class Program
{
    private static void Main(string[] args)
    {
        FizzBuzzService fbService = new FizzBuzzService();
        _ = new List<int>();

        // Collect inputs
        List<int> inputs = GetValidatedInputs(5);

        // Count Fizz, Buzz, and FizzBuzz occurrences
        (int fizzes, int buzzes, int fizzBuzzes) = CountFizzBuzzes(inputs, fbService);

        // Save inputs and display results
        fbService.SaveValueList(inputs);
        Console.WriteLine($"Total Points: {fbService.TalleyPoints()} (Fizz {fizzes}x | Buzz {buzzes}x | FizzBuzzes {fizzBuzzes}x)");
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
    /// Counts the occurrences of Fizz, Buzz, and FizzBuzz in a list of integers.
    /// </summary>
    /// <param name="inputs">The list of integers to evaluate.</param>
    /// <param name="fbService">The FizzBuzz service used for evaluation.</param>
    /// <returns>A tuple containing the counts of Fizz, Buzz, and FizzBuzz.</returns>
    private static (int fizzes, int buzzes, int fizzBuzzes) CountFizzBuzzes(List<int> inputs, FizzBuzzService fbService)
    {
        int fizzes = 0;
        int buzzes = 0;
        int fizzBuzzes = 0;

        foreach (var input in inputs)
        {
            if (fbService.IsFizzBuzz(input))
            {
                fizzBuzzes++;
            }
            else if (fbService.IsBuzz(input))
            {
                buzzes++;
            }
            else if (fbService.IsFizz(input))
            {
                fizzes++;
            }
        }

        return (fizzes, buzzes, fizzBuzzes);
    }
}