using FizzBuzzConsole.model;

namespace FizzBuzzConsole.service;

/// <summary>
/// Interface for the FizzBuzz game service.
/// </summary>
public interface IFizzBuzzService
{
    void SaveValueList(List<int> values);
    int TallyPoints();
    IEnumerable<FizzBuzz> GetSavedValues();
    void ClearPreviousResults();
    (int fizzes, int buzzes, int fizzBuzzes) CountFizzBuzzes();
}