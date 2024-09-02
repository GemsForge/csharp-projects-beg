using Task = TaskTracker.model.Task;
namespace TaskTracker.data
{
    public interface ITaskRepository
    {
        List<Task> LoadTasksFromFile();
        void SaveTasksToFile(List<Task> tasks);
    }
}
