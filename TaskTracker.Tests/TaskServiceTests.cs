using Moq;
using TaskTrackerConsole.data;
using TaskTrackerConsole.model;
using TaskTrackerConsole.services;
using Task = TaskTrackerConsole.model.Task;

namespace TaskTracker.Tests
{
    /// <summary>
    /// Unit tests for TaskService class methods.
    /// </summary>
    public class TaskServiceTests
    {
        private readonly Mock<ITaskManager> _mockTaskManager;
        private readonly TaskService _taskService;

        /// <summary>
        /// Initializes a new instance of the TaskServiceTests class.
        /// </summary>
        public TaskServiceTests()
        {
            // Mock the TaskManager
            _mockTaskManager = new Mock<ITaskManager>();

            // Initialize TaskService with the mocked TaskManager
            _taskService = new TaskService(_mockTaskManager.Object);
        }

        [Fact]
        public void AddNewTask_ValidData_CallsAddTaskOnTaskManager()
        {
            // Arrange
            string description = "New Task";
            Status status = Status.TODO;

            // Act
            _taskService.AddNewTask(description, status);

            // Assert
            _mockTaskManager.Verify(tm => tm.AddTask(It.Is<Task>(t => t.Description == description && t.Status == status)), Times.Once);
        }

        [Fact]
        public void UpdateExistingTask_ValidData_CallsUpdateTaskOnTaskManager()
        {
            // Arrange
            int taskId = 1;
            string newDescription = "Updated Task";
            Status newStatus = Status.COMPLETE;

            // Act
            _taskService.UpdateExistingTask(taskId, newDescription, newStatus);

            // Assert
            _mockTaskManager.Verify(tm => tm.UpdateTask(taskId, It.Is<Task>(t => t.Description == newDescription && t.Status == newStatus)), Times.Once);
        }

        [Fact]
        public void DeleteTaskById_ValidTaskId_CallsDeleteTaskOnTaskManager()
        {
            // Arrange
            int taskId = 1;

            // Act
            _taskService.DeleteTaskById(taskId);

            // Assert
            _mockTaskManager.Verify(tm => tm.DeleteTask(taskId), Times.Once);
        }

        [Fact]
        public void GetAllTasks_ReturnsMappedTaskDtos()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task { Id = 1, Description = "Task 1", Status = Status.TODO, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Task { Id = 2, Description = "Task 2", Status = Status.PENDING, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            };

            _mockTaskManager.Setup(tm => tm.GetTasks()).Returns(tasks);

            // Act
            var result = _taskService.GetAllTasks().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Task 1", result[0].Description);
            Assert.Equal(Status.TODO.ToString(), result[0].Status);
            _mockTaskManager.Verify(tm => tm.GetTasks(), Times.Once);
        }

        [Fact]
        public void GetTaskById_ValidTaskId_ReturnsMappedTaskDto()
        {
            // Arrange
            int taskId = 1;
            var task = new Task { Id = taskId, Description = "Task 1", Status = Status.TODO, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

            _mockTaskManager.Setup(tm => tm.GetTask(taskId)).Returns(task);

            // Act
            var result = _taskService.GetTaskById(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal("Task 1", result.Description);
            _mockTaskManager.Verify(tm => tm.GetTask(taskId), Times.Once);
        }

        [Fact]
        public void GetTaskById_InvalidTaskId_ReturnsNull()
        {
            // Arrange
            int taskId = 999;
            _mockTaskManager.Setup(tm => tm.GetTask(taskId)).Returns((Task)null);

            // Act
            var result = _taskService.GetTaskById(taskId);

            // Assert
            Assert.Null(result);
            _mockTaskManager.Verify(tm => tm.GetTask(taskId), Times.Once);
        }
    }
}
