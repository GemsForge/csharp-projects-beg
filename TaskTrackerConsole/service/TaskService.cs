using CommonLibrary.Data;
using TaskTrackerConsole.model;
using Task = TaskTrackerConsole.model.Task;

namespace TaskTrackerConsole.service
{
    /// <summary>
    /// Manages tasks and provides operations to add, update, delete, and retrieve tasks.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly IGenericRepository<Task> _taskRepo;
        private readonly List<Task> _tasks;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskManager"/> class.
        /// </summary>
        public TaskService(IGenericRepository<Task> taskRepo)
        {
            _taskRepo = taskRepo;
            _tasks = _taskRepo.GetAll().ToList();  // Load tasks from the repository
        }

        /// <summary>
        /// Adds a new task to the task list.
        /// </summary>
        /// <param name="newTask">The task to be added.</param>
        public void AddTask(Task newTask)
        {
            // Generate a new ID and add the task
            int newId = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
            DateTime now = DateTime.Now;

            Task taskToAdd = new()
            {
                Id = newId,
                Description = newTask.Description,
                Status = newTask.Status,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = newTask.CreatedBy
            };

            _tasks.Add(taskToAdd);
            _taskRepo.Add(taskToAdd);  // Add task to the JSON file through the repository
            Console.WriteLine($"Task '{newTask.Description}' added successfully!");
        }

        /// <summary>
        /// Deletes a task from the task list by its identifier.
        /// </summary>
        /// <param name="taskId">The identifier of the task to delete.</param>
        public void DeleteTask(int taskId)
        {
            var taskToDelete = GetTask(taskId);

            if (taskToDelete != null)
            {
                _tasks.Remove(taskToDelete);
                _taskRepo.Remove(taskId);  // Remove from repository
                Console.WriteLine($"Task with ID {taskId} has been deleted successfully!");
            }
            else
            {
                Console.WriteLine($"Task with ID {taskId} not found.");
            }
        }

        /// <summary>
        /// Updates an existing task in the task list.
        /// </summary>
        /// <param name="taskId">The identifier of the task to update.</param>
        /// <param name="updateTask">The task object containing updated information.</param>
        public void UpdateTask(int taskId, Task updateTask)
        {
            var taskToUpdate = GetTask(taskId);

            if (taskToUpdate != null)
            {
                taskToUpdate.Description = updateTask.Description;
                taskToUpdate.Status = updateTask.Status;
                taskToUpdate.UpdatedAt = DateTime.Now;

                _taskRepo.Update(taskId, taskToUpdate);  // Update the repository
                Console.WriteLine($"Task with ID {taskId} has been updated successfully!");
            }
            else
            {
                Console.WriteLine($"Task with ID {taskId} not found.");
            }
        }

        /// <summary>
        /// Gets a task by its identifier.
        /// </summary>
        /// <param name="taskId">The identifier of the task to retrieve.</param>
        /// <returns>The task with the specified identifier, or null if not found.</returns>
        public Task GetTask(int taskId)
        {
            return _tasks.FirstOrDefault(task => task.Id == taskId);
        }

        /// <summary>
        /// Gets all tasks in the task list.
        /// </summary>
        /// <returns>An enumerable list of all tasks.</returns>
        public IEnumerable<Task> GetTasks()
        {
            return _tasks;
        }

        /// <summary>
        /// Prints all tasks to the console.
        /// </summary>
        public void PrintAllTasks()
        {
            Console.WriteLine("All Tasks:");
            foreach (var task in _tasks)
            {
                Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Status: {task.Status}, Created At: {task.CreatedAt}, Updated At: {task.UpdatedAt}");
            }
        }
    }
}
