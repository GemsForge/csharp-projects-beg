using CommonLibrary.Data;
using Moq;
using TaskTrackerConsole.model;
using TaskTrackerConsole.service;
using Task = TaskTrackerConsole.model.Task;

namespace TaskTracker.Tests
{
    /// <summary>
    /// Unit tests for TaskService class methods.
    /// </summary>
    public class TaskServiceTests
    {
        private readonly Mock<IGenericRepository<Task>> _mockTaskRepository;
        private readonly TaskService _taskService;
        private List<Task> _mockTasks;

        /// <summary>
        /// Initializes a new instance of the TaskServiceTests class.
        /// </summary>
        public TaskServiceTests()
        {
            // Set up mock tasks
            _mockTasks = new List<Task>
            {
                new Task { Id = 1, Description = "Task 1", Status = Status.TODO, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Task { Id = 2, Description = "Task 2", Status = Status.PENDING, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Task { Id = 3, Description = "Task 3", Status = Status.COMPLETE, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            };

            // Mock the IGenericRepository<Task>
            _mockTaskRepository = new Mock<IGenericRepository<Task>>();
            _mockTaskRepository.Setup(tr => tr.GetAll()).Returns(_mockTasks);

            // Initialize TaskService with the mocked repository
            _taskService = new TaskService(_mockTaskRepository.Object);
        }

        [Fact]
        public void AddNewTask_ValidData_CallsAddOnRepository()
        {
            // Arrange
            var newTask = new Task { Description = "New Task", Status = Status.TODO };

            // Act
            _taskService.AddTask(newTask);

            // Assert: Verify that the Add method was called on the repository
            _mockTaskRepository.Verify(repo => repo.Add(It.Is<Task>(t => t.Description == "New Task" && t.Status == Status.TODO)), Times.Once);
        }

        [Fact]
        public void UpdateExistingTask_ValidData_CallsUpdateOnRepository()
        {
            // Arrange
            var updatedTask = new Task { Id = 1, Description = "Updated Task", Status = Status.COMPLETE };

            // Act
            _taskService.UpdateTask(1, updatedTask);

            // Assert: Verify that the Update method was called on the repository
            _mockTaskRepository.Verify(repo => repo.Update(1, It.Is<Task>(t => t.Description == "Updated Task" && t.Status == Status.COMPLETE)), Times.Once);
        }

        [Fact]
        public void DeleteTaskById_ValidTaskId_CallsRemoveOnRepository()
        {
            // Arrange
            int taskId = 1;

            // Act
            _taskService.DeleteTask(taskId);

            // Assert: Verify that the Remove method was called on the repository
            _mockTaskRepository.Verify(repo => repo.Remove(taskId), Times.Once);
        }

        [Fact]
        public void GetAllTasks_ReturnsAllTasks()
        {
            // Act
            var tasks = _taskService.GetTasks().ToList();

            // Assert: Verify that the tasks list contains the expected number of tasks
            Assert.Equal(3, tasks.Count);
            Assert.Equal("Task 1", tasks[0].Description);
            _mockTaskRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetTaskById_ValidTaskId_ReturnsCorrectTask()
        {
            // Arrange
            var taskId = 1;
            var task = new Task { Id = taskId, Description = "Task 1", Status = Status.TODO };
            _mockTaskRepository.Setup(repo => repo.GetById(taskId)).Returns(task);

            // Act
            var result = _taskService.GetTask(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal("Task 1", result.Description);
            _mockTaskRepository.Verify(repo => repo.GetById(taskId), Times.Once);
        }

        [Fact]
        public void GetTaskById_InvalidTaskId_ReturnsNull()
        {
            // Arrange
            int taskId = 999;
            _mockTaskRepository.Setup(repo => repo.GetById(taskId)).Returns((Task)null);

            // Act
            var result = _taskService.GetTask(taskId);

            // Assert: Ensure that a null task is returned
            Assert.Null(result);
            _mockTaskRepository.Verify(repo => repo.GetById(taskId), Times.Once);
        }
    }
}
