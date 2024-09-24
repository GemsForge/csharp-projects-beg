using CommonLibrary;
using FizzBuzzConsole.data;
using FizzBuzzConsole.game;
using FizzBuzzConsole.service;

namespace FizzBuzzGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Display the brand logo at the start of the program
            LogoPrinter.DisplayLogo();
            IFizzBuzzDisplay fbDisplay = new FizzBuzzDisplay();
            string filePath = @"C:\Users\Diamond R. Brown\OneDrive\Gem.Professional 🎖️\02 💻 GemsCode\Git Repositories\CSharpProjects\FizzBuzzConsole\data\FizzBuzz.json";
            // Initialize the FizzBuzz repository
            IFizzBuzzRepository fbRepo = new FizzBuzzRepository(filePath);
            // Initialize the FizzBuzz service
            IFizzBuzzService fbService = new FizzBuzzService(fbRepo);

            // Create an instance of the FizzBuzz game with the service
            FizzBuzzArcade game = new(fbService, fbDisplay);

            // Start the FizzBuzz game
            game.StartGame();
        }
    }
}