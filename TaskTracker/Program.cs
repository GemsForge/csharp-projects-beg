using System.Text;
using TaskTracker.data;
using TaskTracker.services;
using TaskTracker.ui;
using Task = TaskTracker.model.Task;

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
            //// Initialize the TaskManager (Data Layer)
            //TaskManager taskManager = new();

            //// Initialize the TaskService (Service Layer) with TaskManager dependency
            //TaskService taskService = new(taskManager);

            //// Initialize the TaskCLI (UI Layer) with TaskService dependency
            //TaskCLI taskCLI = new(taskService);

            //// Run the CLI
            //taskCLI.Run();
   
            TaskManager taskManager = new();

            // Test: Print all tasks (load from file or initialize defaults)
            Console.WriteLine("Initial Task List:");
            taskManager.PrintAllTasks();

            // Test: Add a new task
            Console.WriteLine("\nAdding a new task...");
            Task newTask = new Task
            {
                Description = "Debug the bug that only happens on Fridays.",
                Status = Status.TODO
            };
            taskManager.AddTask(newTask);
            taskManager.PrintAllTasks();

            // Test: Update an existing task
            Console.WriteLine("\nUpdating task with ID 1...");
            Task updatedTask = new Task
            {
                Description = "Complete the project documentation with extra coffee.",
                Status = Status.PENDING
            };
            taskManager.UpdateTask(1, updatedTask);
            taskManager.PrintAllTasks();

            // Test: Delete a task
            Console.WriteLine("\nDeleting task with ID 2...");
            taskManager.DeleteTask(2);
            taskManager.PrintAllTasks();

            // Test: Error handling - Delete non-existent task
            Console.WriteLine("\nAttempting to delete a non-existent task with ID 999...");
            taskManager.DeleteTask(999);
        }
        
    }
}
