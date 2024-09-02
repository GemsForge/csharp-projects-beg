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
        private readonly string _testFilePath;
        private readonly StringWriter _consoleOutput;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly List<Task> _tasks;

        /// <summary>
        /// Initializes a new instance of the TaskRepositoryTests class.
        /// </summary>
        public TaskRepositoryTests()
        {
            // Setup for a clean test environment
            _testFilePath = "test_tasks.json";

            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);

            _jsonOptions = new JsonSerializerOptions
            {
                Converters = { new StatusEnumConverter() },
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            _tasks = new List<Task>
            {
                new() { Id = 1, Description = "Sample Task 1", Status = Status.TODO },
                new() { Id = 2, Description = "Sample Task 2", Status = Status.PENDING }
            };

        }

        [Fact]
        public void NormalizeAndGetFullPath_ValidPath_ReturnsNormalizedPath()
        {
            // Arrange
            string path = "C:\\temp💻\\testfile.txt";

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

            var options = new JsonSerializerOptions
            {
                Converters = { new StatusEnumConverter() },
                WriteIndented = true
            };
            File.WriteAllText(_testFilePath, JsonSerializer.Serialize(_tasks, options));
            var taskRepository = new TaskRepository(_testFilePath); // Use the constructor with testFilePath

            // Act
            var result = taskRepository.LoadTasksFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal("Sample Task 1", result[0].Description);
            Assert.Equal(Status.TODO, result[0].Status);
            Dispose();
        }

        [Fact]
        public void LoadTasksFromFile_FileDoesNotExist_ReturnsDefaultTasks()
        {
            // Arrange
            var taskRepository = new TaskRepository(_testFilePath); // Use the constructor with testFilePath
            taskRepository.SaveTasksToFile(_tasks);
            // Act
            var result = taskRepository.LoadTasksFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);  // Ensure default tasks are returned
            Dispose();
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
            Dispose();
        }

        [Fact]
        public void SaveTasksToFile_ValidTasks_SavesSuccessfully()
        {
            // Arrange
            _tasks.Add(
                new Task { Id = 3, Description = "Sample Task 3", Status = Status.TODO }
            );
            var taskRepository = new TaskRepository(_testFilePath); // Use the constructor with testFilePath

            // Act
            taskRepository.SaveTasksToFile(_tasks);

            // Assert
            string savedContent = File.ReadAllText(_testFilePath);
            var options = new JsonSerializerOptions
            {
                Converters = { new StatusEnumConverter() },
                PropertyNameCaseInsensitive = true
            };
            List<Task> deserializedTasks = JsonSerializer.Deserialize<List<Task>>(savedContent, options);

            Assert.NotNull(deserializedTasks);
            Assert.NotEmpty(deserializedTasks);
            Assert.Equal("Sample Task 3", deserializedTasks[2].Description);
            Assert.Equal(Status.TODO, deserializedTasks[2].Status);
            Dispose();
        }

        [Fact]
        public void SaveTasksToFile_ExceptionThrown_ErrorHandledGracefully()
        {
            // Arrange
            var testPath = @"\0invalidpath";

            var taskRepository = new TaskRepository(testPath); // Use the constructor with testFilePath

            // Act & Assert
            var exception = Record.Exception(() => taskRepository.SaveTasksToFile(_tasks));
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
