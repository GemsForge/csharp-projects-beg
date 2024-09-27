using Moq;
using TaskTrackerConsole.dto;
using TaskTrackerConsole.manager;
using TaskTrackerConsole.model;
using TaskTrackerConsole.ui;

namespace TaskTracker.Tests
{
    public class TaskCLITests : IDisposable
    {
        private readonly Mock<ITaskManager> _mockTaskService;  // Correctly mocking the interface
        private StringWriter _consoleOutput;
        private StringReader _consoleInput;

        public TaskCLITests()
        {
            _mockTaskService = new Mock<ITaskManager>(); // Corrected: Mocking the interface without constructor args
            SetUpConsoleOutput();

        }
        private void SetUpConsoleOutput()
        {
            _consoleOutput = new StringWriter();   // Capture console output 
            Console.SetOut(_consoleOutput);
        }

        private void SetUpConsoleInput(string input)
        {
            _consoleInput = new StringReader(input);  // Set console 
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
            Dispose();
        }

        [Fact]
        public void AddTask_ValidInput_AddsTaskSuccessfully()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskService.Object);
            var input = "Test Task Description\n0\n";  // Simulate user input for description and status
           SetUpConsoleInput(input);

            _mockTaskService.Setup(service => service.AddNewTask("Test Task Description", Status.TODO));

            // Act
            taskCLI.AddTask();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("Task added successfully.", output);
            Dispose();
        }


        [Fact]
        public void DeleteTask_InvalidTaskId_PrintsErrorMessage()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskService.Object);

            // Mock setup for task retrieval to return null
            _mockTaskService.Setup(service => service.GetTaskById(It.IsAny<int>())).Returns((TaskDto)null); // Simulate task not found

            // Simulate user input for invalid task IDs and then a valid one
            var input = "99\n100\n101\n102\n103\n";  // Exceed maxAttempts to trigger the abort message
           SetUpConsoleInput(input);

            // Act
            taskCLI.DeleteTask();

            // Assert
            var output = _consoleOutput.ToString();
            Assert.Contains("Exceeded maximum attempts. Task deletion aborted.", output);  // Check for abort message
            Dispose();
        }


        [Fact]
        public void DeleteTask_ValidTaskId_DeletesTaskSuccessfully()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskService.Object);

            // Mock setup for task retrieval and task deletion
            _mockTaskService.Setup(service => service.GetTaskById(1)).Returns(new TaskDto { Id = 1, Description = "Test Task", Status = "TODO", CreatedAt = "2024-08-25 10:00:00", UpdatedAt = "2024-08-25 10:00:00" });

            // Simulate user input for a valid task ID
            var input = "1\n";
            SetUpConsoleInput(input);

            using (var consoleOutput = new StringWriter()) ;
            // Act
            taskCLI.DeleteTask();

            // Assert
            var output = _consoleOutput.ToString();

            // Check that the valid ID input is processed
            Assert.Contains("Task deleted successfully.", output);

            // Verify that the DeleteTaskById method was called once with the correct valid ID
            _mockTaskService.Verify(service => service.DeleteTaskById(1), Times.Once);
            Dispose();
        }

        [Fact]
        public void UpdateTask_InvalidTaskId_PrintsErrorMessage()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskService.Object);

            // Mock setup for task retrieval to return null
            _mockTaskService.Setup(service => service.GetTaskById(99)).Returns((TaskDto)null); // Simulate task not found

            // Simulate user input for an invalid task ID
            var input = "99\n";
            SetUpConsoleInput(input);

            // Act
            taskCLI.UpdateTask();

            // Assert
            var output = _consoleOutput.ToString();

            // Check that the invalid ID input is handled correctly
            Assert.Contains("Task with ID 99 not found.", output);
            Dispose();
        }

        [Fact]
        public void UpdateTask_ValidTaskIdAndInputs_UpdatesTaskSuccessfully()
        {
            // Arrange
            var taskCLI = new TaskCLI(_mockTaskService.Object);

            // Mock setup for task retrieval and task update
            _mockTaskService.Setup(service => service.GetTaskById(1)).Returns(new TaskDto
            {
                Id = 1,
                Description = "Test Task",
                Status = "TODO",
                CreatedAt = "2024-08-25 10:00:00",
                UpdatedAt = "2024-08-25 10:00:00"
            });

            // Simulate user input for a valid task ID, new description, and new status
            var input = "1\nNew Description\n1\n";  // Valid ID, new description, new status
            SetUpConsoleInput(input);

            // Act
            taskCLI.UpdateTask();

            // Assert
            var output = _consoleOutput.ToString();

            // Check that the valid ID input is processed
            Assert.Contains("Enter new task description (current: Test Task):", output);
            Assert.Contains("Enter new task status (0 = TODO, 1 = PENDING, 2 = COMPLETE) (current: TODO):", output);
            Assert.Contains("Task updated successfully.", output);

            // Verify that the UpdateExistingTask method was called once with the correct parameters
            _mockTaskService.Verify(service => service.UpdateExistingTask(1, "New Description", Status.PENDING), Times.Once);
            Dispose();
        }



        public void Dispose()
        {
            // Dispose of custom StringWriter and StringReader if they were set
            _consoleOutput?.Dispose();
            _consoleInput?.Dispose();

            // Reset Console output and input to default streams
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        }
    }
}
