using Task = TaskTrackerConsole.model.Task;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GemConnectAPI.Mappers.TaskTracker;
using GemConnectAPI.DTO.TaskTracker;
using TaskTrackerConsole.model;
using TaskTrackerConsole.manager;

namespace GemConnectAPI.Controllers.TaskTracker
{
    [Authorize(Policy = "USER")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTrackerController : ControllerBase
    {
        private readonly ITaskManager _taskManager;
        private readonly ITaskMapper _taskMapper;
        private readonly ILogger<TaskTrackerController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTrackerController"/> class.
        /// </summary>
        public TaskTrackerController(ITaskManager taskManager, ITaskMapper taskMapper, ILogger<TaskTrackerController> logger)
        {
            _taskManager = taskManager;
            _taskMapper = taskMapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all tasks.
        /// </summary>
        [HttpGet("tasks")]
        public ActionResult<IEnumerable<TaskDto>> GetTasks()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);  // Get the User Id from the claims

            var tasks = _taskManager.GetAllTasks().Where(t => t.CreatedBy == userId);
            var taskDtos = tasks.Select(task => _taskMapper.MapTaskToDto(task, User.Identity?.Name));

            return Ok(taskDtos);
        }

        /// <summary>
        /// Gets a specific task by ID.
        /// </summary>
        [HttpGet("task/{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _taskManager.GetTaskById(id);
            if (task == null) return HandleTaskNotFound(id);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (task.CreatedBy != userId && !User.IsInRole("ADMIN"))
            {
                return Forbid("You are not allowed to update this task.");
            }

            return Ok($"{task} : {User.Identity?.Name}");
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        [HttpPost]
        public IActionResult AddTask([FromBody] CreateTaskDto taskDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var (statusEnum, errorResponse) = ValidateAndParseStatus(taskDto.Status);
            if (errorResponse != null) return errorResponse;

            var newTask = _taskMapper.MaptoTask(taskDto, userId, statusEnum.Value);
            _taskManager.AddNewTask(newTask.Description, newTask.Status);

            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id });
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] CreateTaskDto taskDto)
        {
            var task = _taskManager.GetTaskById(id);
            if (task == null) return HandleTaskNotFound(id);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (task.CreatedBy != userId && !User.IsInRole("ADMIN"))
            {
                return Forbid("You are not allowed to update this task.");
            }

            var (statusEnum, errorResponse) = ValidateAndParseStatus(taskDto.Status);
            if (errorResponse != null) return errorResponse;
            task.Description = taskDto.Description;
            task.Status = statusEnum.Value;
            _taskManager.UpdateExistingTask(id, task.Description, task.Status);

            var updatedTaskDto = _taskMapper.MapTaskToDto(task, User.Identity?.Name);
            return Ok(updatedTaskDto);
        }

        /// <summary>
        /// Deletes a task by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = _taskManager.GetTaskById(id);
            if (task == null) return HandleTaskNotFound(id);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (task.CreatedBy != userId && !User.IsInRole("ADMIN"))
            {
                return Forbid("You are not allowed to update this task.");
            }

            _taskManager.DeleteTaskById(id);
            return NoContent();
        }

        #region Helpers
        private (Status?, IActionResult) ValidateAndParseStatus(string statusString)
        {
            if (Enum.TryParse(statusString, true, out Status statusEnum))
                return (statusEnum, null);

            _logger.LogError($"Invalid status value '{statusString}' provided.");
            return (null, BadRequest("Invalid status value. Please provide a valid status like 'TODO', 'PENDING', or 'COMPLETE'."));
        }

        private NotFoundObjectResult HandleTaskNotFound(int id)
        {
            _logger.LogWarning($"Task with ID {id} not found.");
            return NotFound($"Task with ID {id} could not be found.");
        }
        #endregion
    }
}
