using Moq;
using TaskTracker.dto;
using TaskTracker.services;
using TaskTracker.ui;

namespace TaskTracker.Tests
{
    public class TaskCLITests : IDisposable
    {
        private readonly Mock<ITaskService> _mockTaskService;  // Correctly mocking the interface
        private readonly StringWriter _consoleOutput;
        private readonly StringReader _consoleInput;

        public TaskCLITests()
        {
            _mockTaskService = new Mock<ITaskService>(); // Corrected: Mocking the interface without constructor args

            _consoleOutput = new StringWriter();  // Capture console output
            Console.SetOut(_consoleOutput);

            _consoleInput = new StringReader("");  // Set console input
            Console.SetIn(_consoleInput);
        }

        [Fact]
        public void ListTasks_NoTasksAvailable_PrintsNoTasksAvailableMessage()
        {
            // Arrange
            _mockTaskService.Setup(service => service.GetAllTasks()).Returns(new List<TaskDto>());
            var taskCLI = new TaskCLI(_mockTaskService.Object);

            // Act
            taskCLI.ListTasks();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("No tasks available.", output);
        }

        [Fact]
        public void ListTasks_WithTasks_PrintsTasksGroupedByStatus()
        {
            // Arrange
            var tasks = new List<TaskDto>
    {
        new() { Id = 1, Description = "Task 1", Status = "TODO", CreatedAt = "2024-08-25 10:00:00", UpdatedAt = "2024-08-25 10:00:00" },
        new() { Id = 2, Description = "Task 2", Status = "PENDING", CreatedAt = "2024-08-26 14:30:00", UpdatedAt = "2024-08-27 09:00:00" },
        new() { Id = 3, Description = "Task 3", Status = "COMPLETE", CreatedAt = "2024-08-27 11:00:00", UpdatedAt = "2024-08-27 11:30:00" }
    };

            // Mock the service to return a list of TaskDto objects
            _mockTaskService.Setup(service => service.GetAllTasks()).Returns(tasks);

            // Inject the mock service into TaskCLI
            var taskCLI = new TaskCLI(_mockTaskService.Object);

            // Act
            taskCLI.ListTasks();

            // Assert
            var output = _consoleOutput.ToString();

            // Debugging Output
            Console.WriteLine("Captured Output:");
            Console.WriteLine(output);

            // Check for status group headers
            Assert.Contains("Status: TODO", output);
            Assert.Contains("Status: PENDING", output);
            Assert.Contains("Status: COMPLETE", output);

            // Check for tasks under their respective group headers
            Assert.Contains("ID: 1, Description: Task 1, Created At: 2024-08-25 10:00:00, Updated At: 2024-08-25 10:00:00", output);
            Assert.Contains("ID: 2, Description: Task 2, Created At: 2024-08-26 14:30:00, Updated At: 2024-08-27 09:00:00", output);
            Assert.Contains("ID: 3, Description: Task 3, Created At: 2024-08-27 11:00:00, Updated At: 2024-08-27 11:30:00", output);
        }

        [Fact]
        public void AddTask_ValidInput_AddsTaskSuccessfully()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskService.Object);
            var input = "Test Task Description\n0\n";  // Simulate user input for description and status
            Console.SetIn(new StringReader(input));

            _mockTaskService.Setup(service => service.AddNewTask("Test Task Description", Status.TODO));

            // Act
            taskCLI.AddTask();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("Task added successfully.", output);
        }

        // More tests for UpdateTask and DeleteTask can be written in a similar way...

        public void Dispose()
        {
            // Cleanup after tests
            _consoleOutput.Dispose();
            _consoleInput.Dispose();
            Console.SetOut(Console.Out);  // Reset console output to default
            Console.SetIn(Console.In);    // Reset console input to default
        }
    }
}
