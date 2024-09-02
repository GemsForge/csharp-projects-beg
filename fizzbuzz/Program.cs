// See https://aka.ms/new-console-template for more information


using fizzbuzz;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Program
{
    private static void Main(string[] args)
    {
        int fizzes = 0;
        int buzzes = 0;
        int fizzBuzzes = 0;

        List<int> inputs = new List<int>();
        FizzBuzzService fbService = new FizzBuzzService();

        //value represent the index 
        var index = 1;

        Console.WriteLine("Enter 5 numbers between 100");
        for (int i = 1; i <= 5; i++)
        {
            // Initialize updating based on current 'i' value
            bool updating = i <= 3;

            //Add a for loop to that loops to 100
            while (updating)
            {

                // Try to parse user input, if parsing fails, the loop continues
                if (!int.TryParse(Console.ReadLine(), out int input))
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }


                var isFizz = fbService.IsFizz(input);
                if (isFizz == true)
                {
                    fizzes++;
                }
                else if (fbService.IsBuzz(input) == true)
                {
                    buzzes++;
                }
                else if (fbService.IsFizzBuzz(input) == true)
                {
                    fizzBuzzes++;
                }
                // Add the valid input to the list
                inputs.Add(input);

                //// Update 'i' if you need based on input or break out of the loop conditionally
                i++; // This will update 'i' in the outer for loop if needed

                // Check the condition again to potentially exit the while loop
                updating = i <= 5;

            }

        }
        fbService.SaveValueList(inputs);

        Console.WriteLine($"Total Points: {fbService.TalleyPoints()} (Fizz {fizzes}x | Buzz {buzzes}x | FizzBuzzes {fizzBuzzes}x)");

        //if (i % 3 == 0 && i % 5 == 0)
        //{
        //    Print(index, fb);
        //}
        //else if (i % 5 == 0)
        //{
        //    Print(index, buzz);
        //}
        //else if (i % 3 == 0)
        //{
        //    Print(index, buzz); 
        //}
        //else { Print(index, $"{i}"); }
        //index++;

        ///<summary>
        ///This method prints a formatted string
        ///</summary>
        static void Print(int index, string text)
        {
            Console.WriteLine($"{index}. {text}");
        }
    }
}