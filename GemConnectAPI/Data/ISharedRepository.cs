namespace GemConnectAPI.Data
{
    public interface ISharedRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T item);
        void Update(int id, T item);
        void Remove(int id);
        void Save();
    }
}
