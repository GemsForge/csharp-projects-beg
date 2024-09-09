using CommonLibrary;
using CommonLibrary.TaskTracker.data;
using TaskTrackerConsole.services;
using TaskTrackerConsole.ui;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskTrackerConsole
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

            string filePath = @"C:\Users\Diamond R. Brown\OneDrive\Gem.Professional 🎖️\02 💻 GemsCode\Git Repositories\CSharpProjects\CommonLibrary\TaskTracker\data\Tasks.json";

            // Instantiate TaskRepository which implements ITaskRepository
            // Initialize the TaskManager (Data Layer)
            TaskManager taskManager = new(new TaskRepository(filePath));

            // Initialize the TaskService (Service Layer) with TaskManager dependency
            TaskService taskService = new(taskManager);

            // Initialize the TaskCLI (UI Layer) with TaskService dependency
            TaskCLI taskCLI = new(taskService);

            // Run the CLI
            taskCLI.Run();


        }
    }
}
