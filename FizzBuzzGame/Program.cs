using FizzBuzzGame.service;
using FizzBuzzGame.game;
using CommonLibrary;

namespace FizzBuzzGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Display the brand logo at the start of the program
            LogoPrinter.DisplayLogo();
            IFizzBuzzDisplay fbDisplay = new FizzBuzzDisplay();

            // Initialize the FizzBuzz service
            IFizzBuzzService fbService = new FizzBuzzService();

            // Create an instance of the FizzBuzz game with the service
            FizzBuzzArcade game = new(fbService, fbDisplay);

            // Start the FizzBuzz game
            game.StartGame();
        }
    }
}