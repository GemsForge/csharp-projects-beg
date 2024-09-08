using System.Text.Json;
using System.Text.Json.Serialization;
using TaskTracker.data;

using Task = TaskTracker.model.Task;

namespace TaskTracker.Tests
{
    /// <summary>
    /// Unit tests for TaskRepository class methods.
    /// </summary>
    public class TaskRepositoryTests : IDisposable
    {
        private readonly string testFilePath = "test_tasks.json";
        private List<Task> _defaultTasks;
        private StringWriter _consoleOutput;
        private TextWriter _originalConsoleOutput;

        public TaskRepositoryTests()
        {
            // Setup default tasks for testing
            _defaultTasks = new List<Task>
            {
                new Task { Id = 1, Description = "Sample Task 1", Status = Status.TODO, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Task { Id = 2, Description = "Sample Task 2", Status = Status.PENDING, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Task { Id = 3, Description = "Sample Task 3", Status = Status.COMPLETE, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            };

            // Initialize console output redirection for capturing messages
            _consoleOutput = new StringWriter();
            _originalConsoleOutput = Console.Out;
            Console.SetOut(_consoleOutput);

            // Ensure the test file is created before each test
            RecreateTestFile();
        }

        public void Dispose()
        {
            // Dispose resources after each test
            _consoleOutput.Dispose();
            Console.SetOut(_originalConsoleOutput);

            // Clean up the test file after tests
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

        /// <summary>
        /// Recreates the test_tasks.json file before each test.
        /// </summary>
        private void RecreateTestFile()
        {
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                WriteIndented = true
            };

            File.WriteAllText(testFilePath, JsonSerializer.Serialize(_defaultTasks, options));
        }

        [Fact]
        public void LoadTasksFromFile_FileExists_ReturnsTasksList()
        {
            // Arrange
            var taskRepository = new TaskRepository(testFilePath); // Use the constructor with testFilePath

            // Act
            var result = taskRepository.LoadTasksFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Sample Task 1", result[0].Description);
            Assert.Equal(Status.TODO, result[0].Status);
        }

        [Fact]
        public void LoadTasksFromFile_FileDoesNotExist_ReturnsDefaultTasks()
        {
            // Arrange
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);

            var taskRepository = new TaskRepository(testFilePath);

            // Act
            var result = taskRepository.LoadTasksFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal("Write the project documentation... or at least start it and then procrastinate with coffee.", result[0].Description);
        }

        [Fact]
        public void SaveTasksToFile_ValidTasks_SavesSuccessfully()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task { Id = 4, Description = "New Task", Status = Status.TODO, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            };
            var taskRepository = new TaskRepository(testFilePath);

            // Act
            taskRepository.SaveTasksToFile(tasks);

            // Assert
            string savedContent = File.ReadAllText(testFilePath);

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                PropertyNameCaseInsensitive = true
            };

            List<Task> deserializedTasks = JsonSerializer.Deserialize<List<Task>>(savedContent, options);

            Assert.NotNull(deserializedTasks);
            Assert.Single(deserializedTasks);
            Assert.Equal("New Task", deserializedTasks[0].Description);
            Assert.Equal(Status.TODO, deserializedTasks[0].Status);
        }

        [Fact]
        public void NormalizeAndGetFullPath_ValidPath_ReturnsNormalizedPath()
        {
            // Arrange
            string path = "C:\\temp\\testfile.txt";

            // Act
            string result = TaskRepository.NormalizeAndGetFullPath(path);

            // Assert
            Assert.Equal(Path.GetFullPath(path), result);
        }

        [Fact]
        public void NormalizeAndGetFullPath_InvalidPath_ThrowsArgumentException()
        {
            // Arrange
            string invalidPath = "\0invalidpath";

            using (var consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);  // Redirect console output to capture it

                // Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => TaskRepository.NormalizeAndGetFullPath(invalidPath));
                Assert.Equal("Illegal characters in path. (Parameter 'path')", exception.Message);

                // Capture and assert the console output
                string output = consoleOutput.ToString().Trim();  // Capture the console output and trim whitespace
                Assert.Contains("Invalid path provided:", output);  // Assert that the output contains the correct message
                Assert.Contains(exception.Message, output);  // Assert that the output contains the exception message
            }

            // Reset the console output to the default
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }




    }
}
