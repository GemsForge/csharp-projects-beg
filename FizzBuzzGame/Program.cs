using FizzBuzzGame.service;
using FizzBuzzGame.game;
using TaskTracker.BrandLogo;

namespace FizzBuzzGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Display the brand logo at the start of the program
            Logo.DisplayLogo();

            // Display the rules of the FizzBuzz game to the console
            FizzBuzzDisplay.DisplayGameRules();

            // Initialize the FizzBuzz service
            IFizzBuzzService fbService = new FizzBuzzService();

            // Create an instance of the FizzBuzz game with the service
            FizzBuzzArcade game = new FizzBuzzArcade(fbService);

            // Start the FizzBuzz game
            game.StartGame();
        }
    }
}