using FizzBuzzConsole.model;

namespace FizzBuzzConsole.data
{
    public interface IFizzBuzzRepository
    {
        List<FizzBuzz> LoadResults();
        void SaveResults(List<FizzBuzz> results);


    }
}
