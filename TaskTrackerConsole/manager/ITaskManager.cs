using Task = TaskTrackerConsole.model.Task;
using TaskTrackerConsole.model;
using TaskTrackerConsole.dto;

namespace TaskTrackerConsole.manager
{
    /// <summary>
    /// Interface for managing tasks and business logic.
    /// </summary>
    public interface ITaskManager
    {
        void AddNewTask(string description, Status status);
        void UpdateExistingTask(int taskId, string newDescription, Status newStatus);
        void DeleteTaskById(int taskId);
        IEnumerable<Task> GetAllTasks();
        Task? GetTaskById(int taskId);
        TaskDto MapToDto(Task task);
    }
}
