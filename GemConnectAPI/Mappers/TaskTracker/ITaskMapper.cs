using GemConnectAPI.DTO.TaskTracker;
using TaskTrackerConsole.model;
using Task = TaskTrackerConsole.model.Task;

namespace GemConnectAPI.Mappers.TaskTracker
{
    public interface ITaskMapper
    {
        Task MaptoTask(TaskDto taskDto, string userId, Status statusEnum);
        TaskDto MapTaskToDto(Task task);
        bool TryParseStatus(string statusString, out Status status);
    }
}