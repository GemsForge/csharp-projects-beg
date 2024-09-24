using FizzBuzzConsole.model;

namespace FizzBuzzConsole.data
{
    public interface IFizzBuzzRepository
    {
        
        List<FizzBuzzGamePlay> LoadResults();
        void SaveResults(List<FizzBuzzGamePlay> gamePlays);


    }
}
