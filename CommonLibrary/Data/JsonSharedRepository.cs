using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommonLibrary.Data
{
    public class JsonSharedRepository<TWrapper, TItem> : ISharedRepository<TWrapper, TItem>
        where TWrapper : class, new()
    {
        private readonly string _filePath;
        private List<TWrapper> _items;

        public JsonSharedRepository(string filePath)
        {
            _filePath = NormalizeAndGetFullPath(filePath);
            _items = LoadFromFile();
        }

        public IEnumerable<TItem> GetAll()
        {
            // Extract the list of items from the wrapper
            var wrapper = _items.FirstOrDefault();
            if (wrapper != null)
            {
                var prop = wrapper.GetType().GetProperty(typeof(List<TItem>).Name.ToLower());
                if (prop != null)
                {
                    return (IEnumerable<TItem>)prop.GetValue(wrapper);
                }
            }
            return new List<TItem>();
        }

        public TItem GetById(int id)
        {
            return GetAll().FirstOrDefault(item => GetId(item) == id);
        }

        public void Add(TItem item)
        {
            var wrapper = _items.FirstOrDefault();
            if (wrapper != null)
            {
                var prop = wrapper.GetType().GetProperty(typeof(List<TItem>).Name.ToLower());
                if (prop != null)
                {
                    var list = (List<TItem>)prop.GetValue(wrapper);
                    list.Add(item);
                    SaveToFile();
                }
            }
        }

        public void Update(int id, TItem item)
        {
            var existingItem = GetById(id);
            if (existingItem != null)
            {
                var wrapper = _items.FirstOrDefault();
                if (wrapper != null)
                {
                    var prop = wrapper.GetType().GetProperty(typeof(List<TItem>).Name.ToLower());
                    if (prop != null)
                    {
                        var list = (List<TItem>)prop.GetValue(wrapper);
                        list.Remove(existingItem);
                        list.Add(item);
                        SaveToFile();
                    }
                }
            }
        }

        public void Remove(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                var wrapper = _items.FirstOrDefault();
                if (wrapper != null)
                {
                    var prop = wrapper.GetType().GetProperty(typeof(List<TItem>).Name.ToLower());
                    if (prop != null)
                    {
                        var list = (List<TItem>)prop.GetValue(wrapper);
                        list.Remove(item);
                        SaveToFile();
                    }
                }
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

        public int GetId(TItem item)
        {
            var prop = item.GetType().GetProperty("Id");
            return (int)(prop?.GetValue(item) ?? 0);
        }

        public List<TWrapper> LoadFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TWrapper>();
            }
            try
            {
                string json = File.ReadAllText(_filePath);

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                return JsonSerializer.Deserialize<List<TWrapper>>(json, options) ?? new List<TWrapper>();
            }
            catch (JsonException jex)
            {
                Console.WriteLine($"Error deserializing items from file: {jex.Message}");
                return new List<TWrapper>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading items from file: {ex.Message}");
                return new List<TWrapper>();
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
