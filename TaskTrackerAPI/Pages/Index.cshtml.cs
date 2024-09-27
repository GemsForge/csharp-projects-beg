using Task = TaskTrackerConsole.model.Task;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTrackerConsole.data;
using TaskTrackerConsole.service;

namespace TaskTrackerAPI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITaskService _taskManager;
        private readonly ITaskRepository _taskRepo;

        public List<Task> Tasks { get; set; } = new();

        public IndexModel()
        {
            string filePath = @"C:\Users\Diamond R. Brown\OneDrive\Gem.Professional 🎖️\02 💻 GemsCode\Git Repositories\CSharpProjects\CommonLibrary\TaskTracker\data\Tasks.json";
            _taskRepo = new TaskRepository(filePath);
            _taskManager = new TaskManager(_taskRepo);
        }

        public void OnGet()
        {
            Tasks = (List<Task>)_taskManager.GetTasks();
        }
    }
}
