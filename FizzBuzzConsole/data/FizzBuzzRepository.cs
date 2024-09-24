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
        public List<FizzBuzzGamePlay> LoadResults()
        {
            if (!File.Exists(_filePath))
            {
                return [];
            }

            var jsonData = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<FizzBuzzGamePlay>>(jsonData) ?? [];
        }

        // Save FizzBuzz gameplay results to the JSON file
        public void SaveResults(List<FizzBuzzGamePlay> gamePlays)
        {
            string jsonData = JsonSerializer.Serialize(gamePlays, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}
