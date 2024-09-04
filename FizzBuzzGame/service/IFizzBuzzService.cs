namespace FizzBuzz.service
{
    /// <summary>
    /// Interface for the FizzBuzz game service.
    /// </summary>
    public interface IFizzBuzzService
    {
        void SaveValueList(List<int> values);
        int TallyPoints();
        IEnumerable<FizzBuzz> GetSavedValues();
    }
}