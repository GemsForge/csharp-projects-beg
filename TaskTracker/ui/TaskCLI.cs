using TaskTracker.services;

namespace TaskTracker.ui
{
    /// <summary>
    /// Class for handling command-line interface interactions for tasks.
    /// </summary>
    public class TaskCLI
    {
        private readonly TaskService _taskService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCLI"/> class.
        /// </summary>
        /// <param name="taskService">The task service responsible for business logic operations.</param>
        public TaskCLI(TaskService taskService)
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
        private void AddTask()
        {
            Console.Write("Enter task description: ");
            string description = Console.ReadLine();

            Console.Write("Enter task status (0 = Todo, 1 = InProgress, 2 = Done): ");
            Status status = (Status)Enum.Parse(typeof(Status), Console.ReadLine());

            _taskService.AddNewTask(description, status);
        }

        /// <summary>
        /// Updates an existing task via user input.
        /// </summary>
        private void UpdateTask()
        {
            Console.Write("Enter task ID to update: ");
            int taskId = int.Parse(Console.ReadLine());

            Console.Write("Enter new task description: ");
            string newDescription = Console.ReadLine();

            Console.Write("Enter new task status (0 = Todo, 1 = InProgress, 2 = Done): ");
            Status newStatus = (Status)Enum.Parse(typeof(Status), Console.ReadLine());

            _taskService.UpdateExistingTask(taskId, newDescription, newStatus);
        }

        /// <summary>
        /// Deletes a task via user input.
        /// </summary>
        private void DeleteTask()
        {
            Console.Write("Enter task ID to delete: ");
            int taskId = int.Parse(Console.ReadLine());

            _taskService.DeleteTaskById(taskId);
        }

        /// <summary>
        /// Lists all tasks via user input.
        /// </summary>
        private void ListTasks()
        {
            var tasks = _taskService.GetAllTasks();
            Console.WriteLine("All tasks:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Status: {task.Status}, Created At: {task.CreatedAt}, Updated At: {task.UpdatedAt}");
            }
        }
    }
}
