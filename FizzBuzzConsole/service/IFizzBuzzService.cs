using FizzBuzzConsole.model;

namespace FizzBuzzConsole.service;

/// <summary>
/// Interface for the FizzBuzz game service.
/// </summary>
public interface IFizzBuzzService
{
    void ClearGamePlay(int gamePlayId);
    (int fizzes, int buzzes, int fizzBuzzes) CountFizzBuzzes(int gamePlayId);
    IEnumerable<FizzBuzzGamePlay> GetGamePlaysForPlayer(int player);
    int TallyPoints(int gamePlayId);
    void SaveGamePlay(string player, List<int> values);
}