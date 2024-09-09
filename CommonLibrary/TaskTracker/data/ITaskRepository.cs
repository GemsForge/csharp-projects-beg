using Task = CommonLibrary.TaskTracker.model.Task;
namespace CommonLibrary.TaskTracker.data
{
    public interface ITaskRepository
    {
        List<Task> LoadTasksFromFile();
        void SaveTasksToFile(List<Task> tasks);
    }
}
