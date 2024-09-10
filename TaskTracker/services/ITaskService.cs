using CommonLibrary.TaskTracker.model;
using Task = CommonLibrary.TaskTracker.model.Task;
using CommonLibrary.TaskTracker.dto;

namespace TaskTrackerConsole.services
{
    /// <summary>
    /// Interface for managing tasks and business logic.
    /// </summary>
    public interface ITaskService
    {
        void AddNewTask(string description, Status status);
        void UpdateExistingTask(int taskId, string newDescription, Status newStatus);
        void DeleteTaskById(int taskId);
        IEnumerable<TaskDto> GetAllTasks();
        TaskDto? GetTaskById(int taskId);
        TaskDto MapToDto(Task task);
    }
}
