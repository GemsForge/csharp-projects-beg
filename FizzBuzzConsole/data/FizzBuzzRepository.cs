using FizzBuzzConsole.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FizzBuzzConsole.data
{
    public class FizzBuzzRepository : IFizzBuzzRepository
    {
        private readonly string _filePath;
        public FizzBuzzRepository(string filePath)
        {
            _filePath = filePath;
        }
        // Load FizzBuzz results from the JSON file
        public List<FizzBuzz> LoadResults()
        {
            if (!File.Exists(_filePath))
            {
                return new List<FizzBuzz>();
            }

            var jsonData = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<FizzBuzz>>(jsonData) ?? new List<FizzBuzz>();
        }

        // Save FizzBuzz results to the JSON file
        public void SaveResults(List<FizzBuzz> results)
        {
            var jsonData = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}
