using CommonLibrary.TaskTracker.data;
using CommonLibrary.TaskTracker.model;
using Moq;
using TaskTrackerConsole.data;
using TaskTrackerConsole.model;
using Task = TaskTrackerConsole.model.Task;

namespace TaskTracker.Tests
{
    /// <summary>
    /// Unit tests for TaskManager class methods using Moq.
    /// </summary>
    public class TaskManagerTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly TaskManager _taskManager;
        private List<Task> _mockTasks;

        /// <summary>
        /// Initializes a new instance of the TaskManagerTests class.
        /// </summary>
        public TaskManagerTests()
        {
            // Set up mock tasks
            _mockTasks = new List<Task>
            {
                new Task { Id = 1, Description = "Sample Task 1", Status = Status.TODO, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Task { Id = 2, Description = "Sample Task 2", Status = Status.PENDING, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Task { Id = 3, Description = "Sample Task 3", Status = Status.COMPLETE, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            };

            // Mock the ITaskRepository
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockTaskRepository.Setup(tr => tr.LoadTasksFromFile()).Returns(_mockTasks);

            // Initialize TaskManager with the mocked ITaskRepository
            _taskManager = new TaskManager(_mockTaskRepository.Object);
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
            _taskManager.AddTask(newTask);

            // Assert: Check that the task was added
            var tasks = _taskManager.GetTasks().ToList();
            Assert.Equal(4, tasks.Count);
            Assert.Contains(tasks, t => t.Description == "New Task");

            // Assert: Check console output
            string output = consoleOutput.ToString();
            Assert.Contains("Task 'New Task' added successfully!", output);
        }

        [Fact]
        public void AddTask_ValidTask_AddsTaskToRepository()
        {
            // Arrange
            var newTask = new Task
            {
                Description = "New Task",
                Status = Status.TODO
            };
            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            //Act
            _taskManager.AddTask(newTask);

            // Assert
            _mockTaskRepository.Verify(repo => repo.SaveTasksToFile(It.Is<List<Task>>(tasks =>
                tasks.Count == 4 &&  // Ensure the count matches all tasks
                tasks[3].Id == 4 && // Ensure that the task has a new task Id.
                tasks[3].Description == "New Task" &&  // Ensure first task is updated
                tasks[3].Status == Status.TODO)), Times.Once);
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
                Description = "Updated Task 1",
                Status = Status.COMPLETE
            };

            // Act
            _taskManager.UpdateTask(1, updatedTask);
            var task = _taskManager.GetTask(1);

            // Assert
            Assert.NotNull(task);
            Assert.Equal("Updated Task 1", task.Description);
            Assert.Equal(Status.COMPLETE, task.Status);
        }

        [Fact]
        public void UpdateTask_ValidTask_UpdatesTaskInRepository()
        {
            // Arrange
            var updatedTask = new Task
            {
                Id = 1,
                Description = "Updated Task 1",
                Status = Status.PENDING
            };

            // Act - Update a task 
            _taskManager.UpdateTask(1, updatedTask);

            // Assert
            _mockTaskRepository.Verify(repo => repo.SaveTasksToFile(It.Is<List<Task>>(tasks =>
                tasks.Count == 3 &&  // Ensure the count matches all tasks
                tasks[0].Description == "Updated Task 1" &&  // Ensure first task is updated
                tasks[0].Status == Status.PENDING)), Times.Once);
        }


        [Fact]
        public void DeleteTask_ValidTaskId_DeletesTaskSuccessfully()
        {
            // Arrange
            int taskIdToDelete = 2;

            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            _taskManager.DeleteTask(taskIdToDelete);

            // Assert: Check that the task was deleted
            var tasks = _taskManager.GetTasks().ToList();
            Assert.Equal(2, tasks.Count); // One task should be deleted
            Assert.DoesNotContain(tasks, t => t.Id == taskIdToDelete);

            // Assert: Check console output
            string output = consoleOutput.ToString();
            Assert.Contains($"Task with ID {taskIdToDelete} has been deleted successfully!", output);
        }
        [Fact]
        public void DeleteTask_InvalidTaskId_DeletesTaskUnsuccessfully()
        {
            // Arrange: Use an invalid task ID that does not exist in the mock data
            int taskIdToDelete = 999; // ID not present in the _mockTasks list

            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            _taskManager.DeleteTask(taskIdToDelete);

            // Assert: Check that the task count remains the same
            var tasks = _taskManager.GetTasks().ToList();
            Assert.Equal(3, tasks.Count); // No task should be deleted since the ID doesn't exist

            // Assert: Check console output for the "not found" message
            string output = consoleOutput.ToString();
            Assert.Contains($"Task with ID {taskIdToDelete} not found.", output);
        }


        [Fact]
        public void DeleteTask_ValidTask_DeletesTaskFromRepository()
        {
            //Act - Delete task
            _taskManager.DeleteTask(2);

            // Assert
            _mockTaskRepository.Verify(repo =>
            repo.SaveTasksToFile(It.Is<List<Task>>(tasks =>
                tasks.Count == 2
                && tasks[1].Id == 3)), Times.Once);
        }


        [Fact]
        public void GetTask_ValidTaskId_ReturnsCorrectTask()
        {
            // Act
            var task = _taskManager.GetTask(1);

            // Assert
            Assert.NotNull(task);
            Assert.Equal(1, task.Id);
            Assert.Equal("Sample Task 1", task.Description);
        }

        [Fact]
        public void GetTasks_ReturnsAllTasks()
        {
            // Act
            var tasks = _taskManager.GetTasks();

            // Assert
            Assert.NotNull(tasks);
            Assert.Equal(3, tasks.Count());
        }
    }
}
