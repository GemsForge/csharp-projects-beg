using Moq;
using TaskTrackerConsole.manager;
using TaskTrackerConsole.model;
using TaskTrackerConsole.service;
using Task = TaskTrackerConsole.model.Task;

namespace TaskTracker.Tests
{
    public class TaskManagerTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly TaskManager _taskManager;
        private List<Task> _mockTasks;

        public TaskManagerTests()
        {
            _mockTasks = new List<Task>
            {
                new Task { Id = 1, Description = "Sample Task 1", Status = Status.TODO, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Task { Id = 2, Description = "Sample Task 2", Status = Status.PENDING, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Task { Id = 3, Description = "Sample Task 3", Status = Status.COMPLETE, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            };

            // Mock the ITaskService instead of ITaskRepository
            _mockTaskService = new Mock<ITaskService>();
            _mockTaskService.Setup(ts => ts.GetTasks()).Returns(_mockTasks);

            // Initialize TaskManager with the mocked ITaskService
            _taskManager = new TaskManager(_mockTaskService.Object);
        }

        [Fact]
        public void AddTask_ValidTask_AddsTaskSuccessfully()
        {
            // Arrange
            var newTask = new Task
            {
                Description = "New Task",
                Status = Status.TODO
            };

            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            _taskManager.AddNewTask(newTask.Description, newTask.Status);

            // Assert: Verify that AddTask was called in the service
            _mockTaskService.Verify(ts => ts.AddTask(It.IsAny<Task>()), Times.Once);

            // Assert: Check console output
            string output = consoleOutput.ToString();
            Assert.Contains("Task 'New Task' added successfully!", output);
        }

        [Fact]
        public void UpdateTask_ValidTask_UpdatesTaskSuccessfully()
        {
            // Arrange
            var updatedTask = new Task
            {
                Id = 1,
                Description = "Updated Task 1",
                Status = Status.COMPLETE
            };

            // Act
            _taskManager.UpdateExistingTask(updatedTask.Id, updatedTask.Description, updatedTask.Status);

            // Assert: Verify that UpdateTask was called in the service
            _mockTaskService.Verify(ts => ts.UpdateTask(updatedTask.Id, It.IsAny<Task>()), Times.Once);
        }

        [Fact]
        public void DeleteTask_ValidTaskId_DeletesTaskSuccessfully()
        {
            // Arrange
            int taskIdToDelete = 2;

            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            _taskManager.DeleteTaskById(taskIdToDelete);

            // Assert: Verify that DeleteTask was called in the service
            _mockTaskService.Verify(ts => ts.DeleteTask(taskIdToDelete), Times.Once);

            // Assert: Check console output
            string output = consoleOutput.ToString();
            Assert.Contains($"Task with ID {taskIdToDelete} has been deleted successfully!", output);
        }

        [Fact]
        public void GetTask_ValidTaskId_ReturnsCorrectTask()
        {
            // Arrange
            int taskId = 1;

            // Act
            var task = _taskManager.GetTaskById(taskId);

            // Assert: Verify that GetTask was called in the service
            _mockTaskService.Verify(ts => ts.GetTask(taskId), Times.Once);
            Assert.NotNull(task);
            Assert.Equal(1, task.Id);
            Assert.Equal("Sample Task 1", task.Description);
        }

        [Fact]
        public void GetTasks_ReturnsAllTasks()
        {
            // Act
            var tasks = _taskManager.GetAllTasks();

            // Assert
            Assert.NotNull(tasks);
            Assert.Equal(3, tasks.Count());
        }
    }
}
