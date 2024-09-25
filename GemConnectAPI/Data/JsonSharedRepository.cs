using Newtonsoft.Json;

namespace GemConnectAPI.Data
{
    public class JsonSharedRepository<T> : ISharedRepository<T>
    {
        private readonly string _filePath;
        private List<T> _items;

        public JsonSharedRepository(string filePath)
        {
            _filePath = filePath;
            _items = LoadFromFile();
        }
        public IEnumerable<T> GetAll() => _items;

        public T GetById(int id)
        {
            return _items.FirstOrDefault(item => GetId(item) == id);
        }

        public void Add(T item)
        {
            _items.Add(item);
            Save();
        }

        public void Update(int id, T item)
        {
            var existingItem = GetById(id);
            if (existingItem != null)
            {
                _items.Remove(existingItem);
                _items.Add(item);
                Save();
            }
        }

        public void Remove(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _items.Remove(item);
                Save();
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private int GetId(T item)
        {
            var prop = item.GetType().GetProperty("Id");
            return (int)(prop?.GetValue(item) ?? 0);
        }
        private List<T> LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }
    }
}
