using Task = TaskTrackerConsole.model.Task;
namespace TaskTrackerConsole.data
{
    public interface ITaskRepository
    {
        List<Task> LoadTasksFromFile();
        void SaveTasksToFile(List<Task> tasks);
    }
}
