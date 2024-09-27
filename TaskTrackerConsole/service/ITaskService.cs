using Task = TaskTrackerConsole.model.Task;
namespace TaskTrackerConsole.service
{
    public interface ITaskService
    {
        void AddTask(Task task);
        void UpdateTask(int taskId, Task task);
        void DeleteTask(int taskId);
        IEnumerable<Task> GetTasks();
        Task? GetTask(int taskId);
    }
}
