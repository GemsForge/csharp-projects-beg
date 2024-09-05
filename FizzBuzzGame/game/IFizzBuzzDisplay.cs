namespace FizzBuzzGame.game
{
    /// <summary>
    /// Interface for displaying the FizzBuzz game and handling user input.
    /// </summary>
    public interface IFizzBuzzDisplay
    {
        void DisplayGameRules();
        List<int> GetValidatedInputs(int numberOfInputs);
        void DisplayResults(int totalPoints, int fizzes, int buzzes, int fizzBuzzes);
    }
}
