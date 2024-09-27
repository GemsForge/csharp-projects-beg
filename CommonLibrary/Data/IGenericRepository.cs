namespace CommonLibrary.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T item);
        void Update(int id, T item);
        void Remove(int id);
        void SaveToFile();
    }
}
