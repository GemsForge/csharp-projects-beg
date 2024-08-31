using TaskTracker.data;
using TaskTracker.dto;
using Task = TaskTracker.model.Task;

namespace TaskTracker.services
{
    /// <summary>
    /// Service class for managing tasks and business logic.
    /// </summary>
    public class TaskService
    {
        private readonly TaskManager _taskManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="taskManager">The task manager responsible for data operations.</param>
        public TaskService(TaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        /// <summary>
        /// Adds a new task with the specified description and status.
        /// </summary>
        /// <param name="description">The task description.</param>
        /// <param name="status">The status of the task.</param>
        public void AddNewTask(string description, Status status)
        {
            Task newTask = new Task
            {
                Description = description,
                Status = status
            };
            _taskManager.AddTask(newTask);
        }

        /// <summary>
        /// Updates an existing task with new details.
        /// </summary>
        /// <param name="taskId">The identifier of the task to update.</param>
        /// <param name="newDescription">The new description of the task.</param>
        /// <param name="newStatus">The new status of the task.</param>
        public void UpdateExistingTask(int taskId, string newDescription, Status newStatus)
        {
            Task updatedTask = new()
            {
                Description = newDescription,
                Status = newStatus
            };
            _taskManager.UpdateTask(taskId, updatedTask);
        }

        /// <summary>
        /// Deletes a task by its identifier.
        /// </summary>
        /// <param name="taskId">The identifier of the task to delete.</param>
        public void DeleteTaskById(int taskId)
        {
            _taskManager.DeleteTask(taskId);
        }

        /// <summary>
        /// Gets all tasks and maps them to a list of TaskDto objects.
        /// </summary>
        /// <returns>A list of TaskDto objects representing all tasks.</returns>
        public IEnumerable<TaskDto> GetAllTasks()
        {
            return _taskManager.GetTasks().Select(task => MapToDto(task));
        }

        /// <summary>
        /// Gets a task by its identifier and maps it to a TaskDto object.
        /// </summary>
        /// <param name="taskId">The identifier of the task to retrieve.</param>
        /// <returns>A TaskDto object if found; otherwise, null.</returns>
        public TaskDto? GetTaskById(int taskId)
        {
            var task = _taskManager.GetTask(taskId);
            return task != null ? MapToDto(task) : null;
        }

        /// <summary>
        /// Maps a Task object to a TaskDto object.
        /// </summary>
        /// <param name="task">The task object to map.</param>
        /// <returns>A TaskDto object with formatted data.</returns>
        private static TaskDto MapToDto(Task task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Description = task.Description,
                Status = task.Status.ToString(),
                CreatedAt = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdatedAt = task.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}
