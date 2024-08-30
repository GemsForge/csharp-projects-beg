using Task = TaskTracker.model.Task;

namespace TaskTracker;

public class TaskManager
{
    private readonly List<Task> tasks;

    public TaskManager()
    {
        tasks = new List<Task>();
    }

    //Add
    public void AddTask(Task newTask)
    {
        //Generate a new id
        int newId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
        DateTime now = DateTime.Now;

        //Create a new Task
        Task taskToAdd = new Task
        {
            Id = newId,
            Description = newTask.Description,
            Status = newTask.Status,
            CreatedAt = now,
            UpdatedAt = now
        };
        // Add the new task to the list
        tasks.Add(taskToAdd);

        //Output success message
        Console.WriteLine($"Task '{newTask.Description}' added successfully!");

    }

    //Delete
    public void DeleteTask(int taskId)
    {
        // Find task in list by id using LINQ
        Task taskToDelete = GetTask(taskId);

        // If deletTask matches an existing id...
        if (taskToDelete != null)
        {
            //Remove task from list by id
            tasks.Remove(taskToDelete);
            //Output DELETION message to console (Successful and Failure)
            Console.WriteLine($"Task with ID {taskId} has been deleted successfully!");
        }
        else
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
        }
    }

    //Update
    public void UpdateTask(int taskId, Task updateTask)
    {
        //Find task in list by id
        var taskToUpdate = GetTask(taskId);

        // If updateTask matches an existing id...
        if (taskToUpdate == null)
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
        }
        else
        {
            //Replace task in list with new task
            if (updateTask != null)
            {
                taskToUpdate.Description = updateTask.Description;
                taskToUpdate.Status = updateTask.Status;
                taskToUpdate.UpdatedAt = DateTime.Now;
                //Output UPDATE message to console
                Console.WriteLine($"Task with ID {taskId} has been updated successfully!");
            }
            else
            {
                Console.WriteLine("The updateTask object provided is null.");
            }
        }

    }


    #region  Helper Methods
    //Get Task by ID
    public Task GetTask(int taskId)
    {
        var task = tasks.FirstOrDefault(existingTask => existingTask.Id == taskId);

        return task;
    }
    //Get list of tasks
    public IEnumerable<Task> GetTasks()
    {
        return tasks;
    }
    public void PrintAllTasks()
    {
        Console.WriteLine("All Tasks:");
        foreach (var task in tasks)
        {
            Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Status: {task.Status}, Created At: {task.CreatedAt}, Updated At: {task.UpdatedAt}");
        }
    }
    #endregion

}