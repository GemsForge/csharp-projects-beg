using CommonLibrary;
using TaskTracker.data;
using TaskTracker.services;
using TaskTracker.ui;

namespace TaskTracker
{
    /// <summary>
    /// Entry point for the TaskTracker application.
    /// Initializes dependencies and starts the command-line interface (CLI).
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method serves as the entry point for the TaskTracker application.
        /// Initializes the data layer, service layer, and CLI for managing tasks.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Display GemsCode Logo in console at the start of the program
            LogoPrinter.DisplayLogo();

            string filePath = @"C:\Users\Diamond R. Brown\OneDrive\Gem.Professional 🎖️\02 💻 GemsCode\Git Repositories\CSharpProjects\TaskTracker\data\Tasks.json";

            if (!File.Exists(filePath))
            {
                Console.WriteLine(filePath, "Path Exists!");
            }
            // Instantiate TaskRepository which implements ITaskRepository
            ITaskRepository taskRepo = new TaskRepository(filePath);
            // Initialize the TaskManager (Data Layer)
            TaskManager taskManager = new(taskRepo);

            // Initialize the TaskService (Service Layer) with TaskManager dependency
            TaskService taskService = new(taskManager);

            // Initialize the TaskCLI (UI Layer) with TaskService dependency
            TaskCLI taskCLI = new(taskService);

            // Run the CLI
            taskCLI.Run();


        }
    }
}
