namespace FizzBuzzGame.game
{
    /// <summary>
    /// Interface for displaying the FizzBuzz game and handling user input.
    /// </summary>
    public interface IFizzBuzzDisplay
    {
        void DisplayGameRules();
        List<int> GetValidatedInputs(int numberOfInputs);
        void DisplayResults( int fizzes, int buzzes, int fizzBuzzes);
        bool AskToPlayAgain();
        void DisplayScore(int score);
        void DisplayFinalScore(int score);



    }
}
