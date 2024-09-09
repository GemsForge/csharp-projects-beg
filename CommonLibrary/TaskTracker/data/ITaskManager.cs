﻿using Task = CommonLibrary.TaskTracker.model.Task;
namespace CommonLibrary.TaskTracker.data
{
    public interface ITaskManager
    {
        void AddTask(Task task);
        void UpdateTask(int taskId, Task task);
        void DeleteTask(int taskId);
        IEnumerable<Task> GetTasks();
        Task? GetTask(int taskId);
    }
}