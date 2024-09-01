using System.Text.Json;
using TaskTracker.data;
using TaskTracker.model;
using Task = TaskTracker.model.Task;

namespace TaskTracker.Tests
{
    /// <summary>
    /// Unit tests for TaskRepository class methods.
    /// </summary>
    public class TaskRepositoryTests : IDisposable
    {
        private readonly string _testFilePath = @"test_tasks.json";
        private readonly StringWriter _consoleOutput;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Initializes a new instance of the TaskRepositoryTests class.
        /// </summary>
        public TaskRepositoryTests()
        {
            // Setup for a clean test environment
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);

            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);

            _jsonOptions = new JsonSerializerOptions
            {
                Converters = { new StatusEnumConverter() },
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
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
        public void NormalizeAndGetFullPath_InvalidPath_ThrowsException()
        {
            // Arrange
            string invalidPath = "\0invalidpath";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => TaskRepository.NormalizeAndGetFullPath(invalidPath));
        }

        [Fact]
        public void LoadTasksFromFile_FileExists_ReturnsTasksList()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task { Id = 1, Description = "Sample Task", Status = Status.TODO }
            };

            var options = new JsonSerializerOptions
            {
                Converters = { new StatusEnumConverter() },
                WriteIndented = true
            };
            File.WriteAllText(_testFilePath, JsonSerializer.Serialize(tasks, options));
            var taskRepository = new TaskRepository(_testFilePath); // Use the constructor with testFilePath

            // Act
            var result = taskRepository.LoadTasksFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Sample Task", result[0].Description);
            Assert.Equal(Status.TODO, result[0].Status);
        }

        [Fact]
        public void LoadTasksFromFile_FileDoesNotExist_ReturnsDefaultTasks()
        {
            // Arrange
            var taskRepository = new TaskRepository(_testFilePath); // Use the constructor with testFilePath

            // Act
            var result = taskRepository.LoadTasksFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);  // Ensure default tasks are returned
        }

        [Fact]
        public void LoadTasksFromFile_InvalidJson_ReturnsEmptyList()
        {
            // Arrange
            File.WriteAllText(_testFilePath, "Invalid JSON Content");
            var taskRepository = new TaskRepository(_testFilePath); // Use the constructor with testFilePath

            // Act
            var result = taskRepository.LoadTasksFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void SaveTasksToFile_ValidTasks_SavesSuccessfully()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task { Id = 1, Description = "Sample Task", Status = Status.TODO }
            };
            var taskRepository = new TaskRepository(_testFilePath); // Use the constructor with testFilePath

            // Act
            taskRepository.SaveTasksToFile(tasks);

            // Assert
            string savedContent = File.ReadAllText(_testFilePath);
            var options = new JsonSerializerOptions
            {
                Converters = { new StatusEnumConverter() },
                PropertyNameCaseInsensitive = true
            };
            List<Task> deserializedTasks = JsonSerializer.Deserialize<List<Task>>(savedContent, options);

            Assert.NotNull(deserializedTasks);
            Assert.Single(deserializedTasks);
            Assert.Equal("Sample Task", deserializedTasks[0].Description);
            Assert.Equal(Status.TODO, deserializedTasks[0].Status);
        }

        [Fact]
        public void SaveTasksToFile_ExceptionThrown_ErrorHandledGracefully()
        {
            // Arrange
            var taskRepository = new TaskRepository(@"C:\Invalid\Path\test_tasks.json"); // Set an invalid path
            var tasks = new List<Task>
    {
        new() { Id = 1, Description = "Sample Task", Status = Status.TODO }
    };

            // Act & Assert
            var exception = Record.Exception(() => taskRepository.SaveTasksToFile(tasks));
            Assert.NotNull(exception); // Assert that an exception occurs
        }


        /// <summary>
        /// Cleanup after each test to ensure a clean environment.
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);

            _consoleOutput.Dispose();
            Console.SetOut(Console.Out);  // Reset the console output to default
        }
    }
}
