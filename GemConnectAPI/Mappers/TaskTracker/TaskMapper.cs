using GemConnectAPI.DTO.TaskTracker;
using TaskTrackerConsole.model;
using Task = TaskTrackerConsole.model.Task;

namespace GemConnectAPI.Mappers.TaskTracker
{
    public class TaskMapper : ITaskMapper
    {
        public Task MaptoTask(TaskDto taskDto, int userId, Status statusEnum)
        {
            return new Task
            {
                Description = taskDto.Description,
                Status = statusEnum,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };
        }

        public Task MaptoTask(CreateTaskDto createTaskDto, int userId, Status statusEnum)
        {
            return new Task
            {
                Description = createTaskDto.Description,
                Status = statusEnum,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId  // Automatically set the userId based on the authenticated user
            };
        }


        public TaskDto MapTaskToDto(Task task, string username)
        {
            return new TaskDto
            {
                Id = task.Id,
                Description = task.Description,
                Status = task.Status.ToString(),
                CreatedAt = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdatedAt = task.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                CreatedBy = username,
            };
        }
        public bool TryParseStatus(string statusString, out Status status) => Enum.TryParse(statusString, true, out status);
    }
}
