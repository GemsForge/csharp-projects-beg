using Task = CommonLibrary.TaskTracker.model.Task;

namespace CommonLibrary.TaskTracker.data
{
    /// <summary>
    /// Manages tasks and provides operations to add, update, delete, and retrieve tasks.
    /// </summary>
    public class TaskManager : ITaskManager
    {
        private readonly List<Task> _tasks;
        private readonly ITaskRepository _taskRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="TaskManager"/> class.
        /// Pre-populates the task list with some sample tasks.
        /// </summary>
        /// <param name="filePath">The path to the JSON file for storing tasks.</param>
        public TaskManager(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            _tasks = _taskRepository.LoadTasksFromFile();  // Load tasks using TaskRepository
        }

        /// <summary>
        /// Adds a new task to the task list.
        /// </summary>
        /// <param name="newTask">The task to be added.</param>
        public void AddTask(Task newTask)
        {
            // Generate a new id
            int newId = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
            DateTime now = DateTime.Now;

            // Create a new Task
            Task taskToAdd = new Task
            {
                Id = newId,
                Description = newTask.Description,
                Status = newTask.Status,
                CreatedAt = now,
                UpdatedAt = now
            };

            // Add the new task to the list
            _tasks.Add(taskToAdd);

            // Add the new task to the json file
            UpdateTaskListJson();

            // Output success message
            Console.WriteLine($"Task '{newTask.Description}' added successfully!");
        }

        /// <summary>
        /// Deletes a task from the task list by its identifier.
        /// </summary>
        /// <param name="taskId">The identifier of the task to delete.</param>
        public void DeleteTask(int taskId)
        {
            // Find task in list by id using LINQ
            Task taskToDelete = GetTask(taskId);

            // If taskToDelete matches an existing id...
            if (taskToDelete != null)
            {
                // Remove task from list by id
                _tasks.Remove(taskToDelete);

                // Remove task from the json file
                UpdateTaskListJson();

                // Output DELETION message to console (Successful and Failure)
                Console.WriteLine($"Task with ID {taskId} has been deleted successfully!");
            }
            else
            {
                Console.WriteLine($"Task with ID {taskId} not found.");
            }
        }

        /// <summary>
        /// Updates an existing task in the task list.
        /// </summary>
        /// <param name="taskId">The identifier of the task to update.</param>
        /// <param name="updateTask">The task object containing updated information.</param>
        public void UpdateTask(int taskId, Task updateTask)
        {
            // Find task in list by id
            var taskToUpdate = GetTask(taskId);

            // If updateTask matches an existing id...
            if (taskToUpdate == null)
            {
                Console.WriteLine($"Task with ID {taskId} not found.");
            }
            else
            {
                // Replace task in list with new task
                if (updateTask != null)
                {
                    taskToUpdate.Description = updateTask.Description;
                    taskToUpdate.Status = updateTask.Status;
                    taskToUpdate.UpdatedAt = DateTime.Now;

                    // Update the Task List JSON file
                    UpdateTaskListJson();
                    // Output UPDATE message to console
                    Console.WriteLine($"Task with ID {taskId} has been updated successfully!");
                }
                else
                {
                    Console.WriteLine("The updateTask object provided is null.");
                }
            }
        }

        #region Helper Methods

        /// <summary>
        /// Gets a task by its identifier.
        /// </summary>
        /// <param name="taskId">The identifier of the task to retrieve.</param>
        /// <returns>The task with the specified identifier, or null if not found.</returns>
        public Task GetTask(int taskId)
        {
            var task = _tasks.FirstOrDefault(existingTask => existingTask.Id == taskId);
            return task;
        }

        /// <summary>
        /// Gets all tasks in the task list.
        /// </summary>
        /// <returns>An enumerable list of all tasks.</returns>
        public IEnumerable<Task> GetTasks()
        {
            return _tasks;
        }
        /// <summary>
        /// Update task list in JSON file.
        /// </summary>
        private void UpdateTaskListJson()
        {
            // Add the new task to the json file
            _taskRepository.SaveTasksToFile(_tasks);
        }

        /// <summary>
        /// Prints all tasks to the console.
        /// </summary>
        public void PrintAllTasks()
        {
            Console.WriteLine("All Tasks:");
            foreach (var task in _tasks)
            {
                Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Status: {task.Status}, Created At: {task.CreatedAt}, Updated At: {task.UpdatedAt}");
            }
        }

        #endregion
    }
}
