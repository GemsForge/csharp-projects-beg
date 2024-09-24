using System.Text;
using System.Text.Json;
using TaskTrackerConsole.model;
using Task = TaskTrackerConsole.model.Task;

namespace TaskTrackerConsole.data
{
    /// <summary>
    /// Handles loading and saving tasks to and from a JSON file.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ITaskRepository"/> class.
        /// </summary>
        /// <param name="filePath">The path to the JSON file for storing tasks.</param>
        public TaskRepository(string filePath)
        {
            _filePath = NormalizeAndGetFullPath(filePath);
        }

        ///<summary>
        /// Normalizes the file path to handle special characters and Unicode correctly,
        /// and gets the full absolute path.
        /// </summary>
        /// <param name="path">The file path to normalize and get the full path.</param>
        /// <returns>The normalized and full absolute file path.</returns>
        public static string NormalizeAndGetFullPath(string path)
        {
            try
            {
                // Normalize the path to Form C (Canonical Decomposition followed by Canonical Composition)
                path = path.Normalize(NormalizationForm.FormC);

                // Get the full absolute path
                return Path.GetFullPath(path);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine($"Invalid path provided: {ae.Message}");
                throw;  // Rethrow the exception to be handled by the caller or test case
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing the file path: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Loads tasks from the JSON file.
        /// </summary>
        /// <returns>A list of tasks loaded from the file.</returns>
        public List<Task> LoadTasksFromFile()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Tasks file not found. Initializing with a pre-populated list.");
                List<Task> initialTasks = InitializeDefaultTasks();
                return initialTasks;
            }

            try
            {
                //Read the JSON File
                string json = File.ReadAllText(_filePath);
                // Log the JSON content for debugging purposes
                Console.WriteLine("JSON content read from file:");
                //Console.WriteLine(json);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new StatusEnumConverter() }  // Use the custom converter for enum
                };
                //Return deserialize JSON into a list of Task objects
                List<Task> taskList = JsonSerializer.Deserialize<List<Task>>(json, options) ?? new List<Task>();
                Console.WriteLine($"Tasks loaded successfully from the file. Total tasks: {taskList.Count}");
                return taskList;
            }
            catch (JsonException jex)
            {
                Console.WriteLine($"Error deserializing tasks from file: {jex.Message}");
                return new List<Task>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading tasks from file: {ex.Message}");
                return new List<Task>();
            }
        }
        /// <summary>
        /// Initializes the task list with some default tasks.
        /// </summary>
        private static List<Task> InitializeDefaultTasks()
        {
            return new List<Task>
            {
                new Task
                {
                    Id = 1,
                    Description = "Write the project documentation... or at least start it and then procrastinate with coffee.",
                    Status = Status.TODO,
                    CreatedAt = DateTime.Parse("2024-08-25T10:00:00"),
                    UpdatedAt = DateTime.Parse("2024-08-25T10:00:00")
                },
                new Task
                {
                    Id = 2,
                    Description = "Review code for module A and pretend to understand every line like a pro.",
                    Status = Status.PENDING,
                    CreatedAt = DateTime.Parse("2024-08-26T14:30:00"),
                    UpdatedAt = DateTime.Parse("2024-08-27T09:00:00")
                },
                new Task
                {
                    Id = 3,
                    Description = "Test new features in module B. Brace yourself for a bug hunt adventure!",
                    Status = Status.COMPLETE,
                    CreatedAt = DateTime.Parse("2024-08-24T08:45:00"),
                    UpdatedAt = DateTime.Parse("2024-08-25T16:20:00")
                }
            };
        }

        /// <summary>
        /// Saves tasks to the JSON file.
        /// </summary>
        /// <param name="tasks">The list of tasks to save.</param>
        public void SaveTasksToFile(List<Task> tasks)
        {
            try
            {
                // Configure JSON options to use string values for enums
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new StatusEnumConverter() }
                };
                string json = JsonSerializer.Serialize(tasks, options);
                File.WriteAllText(_filePath, json);
                Console.WriteLine("Tasks saved successfully to the file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks to file: {ex.Message}");
                throw; // Rethrow the exception to allow the test to catch it
            }
        }
    }
}
