using CommonLibrary.TaskTracker.model;
using TaskTrackerConsole.dto;
using TaskTrackerConsole.services;

namespace TaskTrackerConsole.ui
{
    /// <summary>
    /// Class for handling command-line interface interactions for tasks.
    /// </summary>
    public class TaskCLI
    {
        private readonly ITaskService _taskService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCLI"/> class.
        /// </summary>
        /// <param name="taskService">The task service responsible for business logic operations.</param>
        public TaskCLI(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Runs the command-line interface for task management.
        /// </summary>
        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nChoose an action: 1. Add, 2. Update, 3. Delete, 4. List, 5. Exit");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        UpdateTask();
                        break;
                    case "3":
                        DeleteTask();
                        break;
                    case "4":
                        ListTasks();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input, try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Adds a new task via user input.
        /// </summary>
        public void AddTask()
        {
            Console.Write("Enter task description: ");
            string description = Console.ReadLine();

            Status status;
            while (true)
            {
                Console.Write($"Enter task status (0 = {Status.TODO}, 1 = {Status.PENDING}, 2 = {Status.COMPLETE}): ");
                if (Enum.TryParse(Console.ReadLine(), out status) && Enum.IsDefined(typeof(Status), status))
                {
                    break;
                }
                Console.WriteLine("Invalid status. Please enter a valid number (0, 1, or 2).");
            }

            _taskService.AddNewTask(description, status);
            Console.WriteLine("Task added successfully.");
        }

        /// <summary>
        /// Updates an existing task via user input.
        /// </summary>
        public void UpdateTask()
        {
            int taskId;
            TaskDto existingTask = null;

            // Loop to get a valid task ID and ensure the task exists
            while (true)
            {
                Console.Write("Enter task ID to update: ");
                if (!int.TryParse(Console.ReadLine(), out taskId))
                {
                    Console.WriteLine("Invalid task ID. Please enter a valid number.");
                    continue; // Continue the loop to prompt again
                }

                existingTask = _taskService.GetTaskById(taskId);
                if (existingTask == null)
                {
                    Console.WriteLine($"Task with ID {taskId} not found.");
                    return; // Return if task not found to avoid infinite loop in testing
                }

                break; // Exit the loop if a valid task ID is entered and the task is found
            }

            bool updating = true;

            // Main loop for updating task details
            while (updating)
            {
                Console.Write($"Enter new task description (current: {existingTask.Description}): ");
                string newDescription = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newDescription))
                {
                    newDescription = existingTask.Description;  // Retain existing description if input is blank
                }

                // Convert the string status from TaskDto to Status enum
                if (!Enum.TryParse<Status>(existingTask.Status, true, out Status currentStatus))
                {
                    Console.WriteLine("Invalid current status. Unable to update the task.");
                    return;  // Exit if the current status cannot be parsed
                }

                Status newStatus;

                // Loop until a valid status is entered
                while (true)
                {
                    Console.Write($"Enter new task status (0 = {Status.TODO}, 1 = {Status.PENDING}, 2 = {Status.COMPLETE}) (current: {currentStatus}): ");
                    string statusInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(statusInput))
                    {
                        newStatus = currentStatus;  // Retain existing status if input is blank
                        break;
                    }

                    // Attempt to parse the input as an integer
                    if (int.TryParse(statusInput, out int statusValue) &&
                        Enum.IsDefined(typeof(Status), statusValue))
                    {
                        newStatus = (Status)statusValue;  // Convert integer to enum value
                        break;
                    }

                    Console.WriteLine("Invalid status. Please enter a valid number (0, 1, or 2).");
                }

                // Update the task with the new description and status
                _taskService.UpdateExistingTask(taskId, newDescription, newStatus);
                Console.WriteLine("Task updated successfully.");
                updating = false; // Exit the update loop after a successful update
            }
        }


        /// <summary>
        /// Deletes a task via user input.
        /// </summary>
        public void DeleteTask()
        {
            int taskId;
            int attempts = 0;  // Track the number of attempts
            int maxAttempts = 5;  // Set a maximum number of attempts to prevent infinite loop

            // Loop to get a valid task ID and ensure the task exists
            while (attempts < maxAttempts)
            {
                Console.Write("Enter task ID to delete: ");
                attempts++;

                // Validate task ID input
                if (!int.TryParse(Console.ReadLine(), out taskId))
                {
                    Console.WriteLine("Invalid task ID. Please enter a valid number.");
                    continue; // Continue the loop to prompt again
                }

                // Retrieve task to check if it exists
                var taskToBeDeleted = _taskService.GetTaskById(taskId);
                if (taskToBeDeleted == null)
                {
                    Console.WriteLine($"Task ID {taskId} was not found. Enter another ID.");
                    continue; // Continue the loop to prompt again
                }

                // If task is found, delete it and break out of the loop
                _taskService.DeleteTaskById(taskId);
                Console.WriteLine("Task deleted successfully.");
                return;  // Exit the method after a successful deletion
            }

            // If max attempts exceeded, notify the user
            Console.WriteLine("Exceeded maximum attempts. Task deletion aborted.");
        }




        /// <summary>
        /// Lists all tasks via user input.
        /// </summary>
        public void ListTasks()
        {
            var tasks = _taskService.GetAllTasks();  // Ensure this returns IEnumerable<TaskDto>

            // Use .Count() method to count the number of elements in the IEnumerable
            if (!tasks.Any())
            {
                Console.WriteLine("No tasks available.");
                return;
            }
            // Group tasks by status using LINQ
            var groupedTasks = tasks.GroupBy(task => task.Status);

            Console.WriteLine("All tasks grouped by status:");

            foreach (var group in groupedTasks)
            {
                Console.WriteLine($"\nStatus: {group.Key}");  // Print the status heading

                foreach (var task in group)
                {
                    Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Created At: {task.CreatedAt}, Updated At: {task.UpdatedAt}");
                }
            }
        }
    }
}
