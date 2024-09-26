using CommonLibrary;
using CommonLibrary.Data;
using FizzBuzzConsole.game;
using FizzBuzzConsole.model;
using FizzBuzzConsole.service;

namespace FizzBuzzConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Display the brand logo at the start of the program
            LogoPrinter.DisplayLogo();
            IFizzBuzzDisplay fbDisplay = new FizzBuzzDisplay();
            //string filePath = @"C:\Users\Diamond R. Brown\OneDrive\Gem.Professional 🎖️\02 💻 GemsCode\Git Repositories\CSharpProjects\FizzBuzzConsole\data\FizzBuzz.json";
            string filePath = @"C:\Users\Diamond R. Brown\OneDrive\Gem.Professional 🎖️\02 💻 GemsCode\Git Repositories\CSharpProjects\CommonLibrary\Data\SharedData.json";
            // Initialize the FizzBuzz repository
            // IFizzBuzzRepository fbRepo = new FizzBuzzRepository(filePath);
            ISharedRepository<FizzBuzzWrapper, FizzBuzzGamePlay> fbRepo = new JsonSharedRepository<FizzBuzzWrapper, FizzBuzzGamePlay>(filePath);
            // Initialize the FizzBuzz service
            IFizzBuzzService fbService = new FizzBuzzService(fbRepo);
            string playerId = "4";
            // Create an instance of the FizzBuzz game with the service
            FizzBuzzArcade game = new(fbService, fbDisplay, playerId);

            // Start the FizzBuzz game
            game.StartGame();
        }
    }
}