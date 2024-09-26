namespace CommonLibrary.Data
{
    public interface ISharedRepository<TWrapper, TItem>
    {
        IEnumerable<TItem> GetAll();
        TItem GetById(int id);
        void Add(TItem item);
        void Update(int id, TItem item);
        void Remove(int id);
        void SaveToFile();
        int GetId(TItem item);
        List<TWrapper> LoadFromFile();  // Load the entire wrapper list from file
    }
}
