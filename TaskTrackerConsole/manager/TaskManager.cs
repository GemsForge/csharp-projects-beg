using Task = TaskTrackerConsole.model.Task;
using TaskTrackerConsole.model;
using TaskTrackerConsole.dto;
using TaskTrackerConsole.service;

namespace TaskTrackerConsole.manager
{
    /// <summary>
    /// Service class for managing tasks and business logic.
    /// </summary>
    public class TaskManager : ITaskManager
    {

        private readonly ITaskService _taskService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="taskManager">The task manager responsible for data operations.</param>
        public TaskManager(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Adds a new task with the specified description and status.
        /// </summary>
        /// <param name="description">The task description.</param>
        /// <param name="status">The status of the task.</param>
        public void AddNewTask(string description, Status status)
        {
            Task newTask = new()
            {
                Description = description,
                Status = status
            };
            _taskService.AddTask(newTask);

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
            _taskService.UpdateTask(taskId, updatedTask);
        }

        /// <summary>
        /// Deletes a task by its identifier.
        /// </summary>
        /// <param name="taskId">The identifier of the task to delete.</param>
        public void DeleteTaskById(int taskId)
        {
            _taskService.DeleteTask(taskId);
        }

        /// <summary>
        /// Gets all tasks and maps them to a list of TaskDto objects.
        /// </summary>
        /// <returns>A list of TaskDto objects representing all tasks.</returns>
        public IEnumerable<Task> GetAllTasks()
        {
            return _taskService.GetTasks();
            //return _taskService.GetTasks().Select(task => MapToDto(task));
        }

        /// <summary>
        /// Gets a task by its identifier and maps it to a TaskDto object.
        /// </summary>
        /// <param name="taskId">The identifier of the task to retrieve.</param>
        /// <returns>A TaskDto object if found; otherwise, null.</returns>
        public Task? GetTaskById(int taskId)
        {
            return _taskService.GetTask(taskId);
            //return task != null ? MapToDto(task) : null;
        }

        /// <summary>
        /// Maps a Task object to a TaskDto object.
        /// </summary>
        /// <param name="task">The task object to map.</param>
        /// <returns>A TaskDto object with formatted data.</returns>
        public TaskDto MapToDto(Task task)
        {
            return new TaskDto
            (
                task.Id,
                task.Description,
                task.Status.ToString(),
                task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                task.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                task.CreatedBy.ToString()
            )
            {
                Description = task.Description,
                Status = task.Status.ToString()
            };
        }
    }
}
