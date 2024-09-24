using FizzBuzzConsole.model;

namespace FizzBuzzConsole.manager
{
    public interface IFizzBuzzManager
    {
        FizzBuzzGuess DetermineGuess(int value);
        int CalculatePoints(FizzBuzzGuess guess);
    }
}
