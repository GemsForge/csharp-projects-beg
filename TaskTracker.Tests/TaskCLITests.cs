using Moq;
using TaskTrackerConsole.manager;
using TaskTrackerConsole.model;
using TaskTrackerConsole.ui;
using Task = TaskTrackerConsole.model.Task;

namespace TaskTracker.Tests
{
    public class TaskCLITests : IDisposable
    {
        private readonly Mock<ITaskManager> _mockTaskManager;
        private StringWriter _consoleOutput;
        private StringReader _consoleInput;

        public TaskCLITests()
        {
            _mockTaskManager = new Mock<ITaskManager>();
            SetUpConsoleOutput();
        }

        private void SetUpConsoleOutput()
        {
            _consoleOutput = new StringWriter();   // Capture console output
            Console.SetOut(_consoleOutput);
        }

        private void SetUpConsoleInput(string input)
        {
            _consoleInput = new StringReader(input);  // Set console input
            Console.SetIn(_consoleInput);
        }

        [Fact]
        public void ListTasks_NoTasksAvailable_PrintsNoTasksAvailableMessage()
        {
            // Arrange
            _mockTaskManager.Setup(service => service.GetAllTasks()).Returns(new List<Task>());
            var taskCLI = new TaskCLI(_mockTaskManager.Object);

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
            var tasks = new List<Task>
    {
        new() { Id = 1, Description = "Task 1", Status = Status.TODO, CreatedAt =DateTime.Parse("2024-08-25 10:00:00"), UpdatedAt = DateTime.Parse("2024-08-25 10:00:00"), CreatedBy = 1 },
        new() { Id = 2, Description = "Task 2", Status = Status.PENDING, CreatedAt = DateTime.Parse("2024-08-26 14:30:00"), UpdatedAt = DateTime.Parse("2024-08-27 09:00:00"), CreatedBy = 2},
        new() { Id = 3, Description = "Task 3", Status = Status.COMPLETE, CreatedAt = DateTime.Parse("2024-08-27 11:00:00"), UpdatedAt = DateTime.Parse("2024-08-27 11:30:00"), CreatedBy = 3 }
    };

            // Mock the service to return a list of TaskDto objects
            _mockTaskManager.Setup(service => service.GetAllTasks()).Returns(tasks);

            var taskCLI = new TaskCLI(_mockTaskManager.Object);

            // Act
            taskCLI.ListTasks();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("Status: TODO", output);
            Assert.Contains("ID: 1, Description: Task 1, Created By: 1", output);  // Checking CreatedBy property
            Assert.Contains("ID: 2, Description: Task 2, Created By: 2", output);
            Assert.Contains("ID: 3, Description: Task 3, Created By: 3", output);
        }


        [Fact]
        public void AddTask_ValidInput_AddsTaskSuccessfully()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskManager.Object);
            var input = "Test Task Description\n0\n";  // Simulate user input for description and status
            SetUpConsoleInput(input);

            _mockTaskManager.Setup(service => service.AddNewTask("Test Task Description", Status.TODO));

            // Act
            taskCLI.AddTask();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("Task added successfully.", output);
        }

        [Fact]
        public void DeleteTask_InvalidTaskId_PrintsErrorMessage()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskManager.Object);
            _mockTaskManager.Setup(service => service.GetTaskById(It.IsAny<int>())).Returns((Task)null); // Simulate task not found

            var input = "99\n100\n101\n102\n103\n";  // Exceed maxAttempts to trigger the abort message
            SetUpConsoleInput(input);

            // Act
            taskCLI.DeleteTask();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("Exceeded maximum attempts. Task deletion aborted.", output);
        }


        [Fact]
        public void DeleteTask_ValidTaskId_DeletesTaskSuccessfully()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskManager.Object);
            _mockTaskManager.Setup(service => service.GetTaskById(1)).Returns(new Task { Id = 1, Description = "Test Task", Status = Status.TODO, CreatedBy = 1 });

            var input = "1\n";
            SetUpConsoleInput(input);

            // Act
            taskCLI.DeleteTask();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("Task deleted successfully.", output);
            _mockTaskManager.Verify(service => service.DeleteTaskById(1), Times.Once);
        }

        [Fact]
        public void UpdateTask_InvalidTaskId_PrintsErrorMessage()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskManager.Object);
            _mockTaskManager.Setup(service => service.GetTaskById(99)).Returns((Task)null); // Simulate task not found

            var input = "99\n";
            SetUpConsoleInput(input);

            // Act
            taskCLI.UpdateTask();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("Task with ID 99 not found.", output);
        }


        [Fact]
        public void UpdateTask_ValidTaskIdAndInputs_UpdatesTaskSuccessfully()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskManager.Object);
            _mockTaskManager.Setup(service => service.GetTaskById(1)).Returns(new Task
            {
                Id = 1,
                Description = "Test Task",
                Status = Status.TODO,
                CreatedBy = 1
            });

            var input = "1\nNew Description\n1\n";  // Valid ID, new description, new status
            SetUpConsoleInput(input);

            // Act
            taskCLI.UpdateTask();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("Task updated successfully.", output);
            _mockTaskManager.Verify(service => service.UpdateExistingTask(1, "New Description", Status.PENDING), Times.Once);
        }

        public void Dispose()
        {
            _consoleOutput?.Dispose();
            _consoleInput?.Dispose();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        }
    }
}
