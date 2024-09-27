using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommonLibrary.Data
{
    public class GenericJsonRepository<T> : IGenericRepository<T> where T : class, new()
    {
        private readonly string _filePath;
        private List<T> _items;

        public GenericJsonRepository(string filePath)
        {
            _filePath = NormalizeAndGetFullPath(filePath);
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
            SaveToFile();
        }

        public void Update(int id, T item)
        {
            var existingItem = GetById(id);
            if (existingItem != null)
            {
                _items.Remove(existingItem);
                _items.Add(item);
                SaveToFile();
            }
        }

        public void Remove(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _items.Remove(item);
                SaveToFile();
            }
        }

        public void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                };
                string json = JsonSerializer.Serialize(_items, options);
                File.WriteAllText(_filePath, json);
                Console.WriteLine("Items saved successfully to the file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving items to file: {ex.Message}");
                throw;
            }
        }

        public int GetId(T item)
        {
            var prop = item.GetType().GetProperty("Id");
            return (int)(prop?.GetValue(item) ?? 0);
        }

        private List<T> LoadFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }
            try
            {
                string json = File.ReadAllText(_filePath);
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };
                return JsonSerializer.Deserialize<List<T>>(json, options) ?? new List<T>();
            }
            catch (JsonException jex)
            {
                Console.WriteLine($"Error deserializing items from file: {jex.Message}");
                return new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading items from file: {ex.Message}");
                return new List<T>();
            }
        }

        private string NormalizeAndGetFullPath(string path)
        {
            try
            {
                path = path.Normalize(NormalizationForm.FormC);
                return Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing the file path: {ex.Message}");
                throw;
            }
        }
    }
}
