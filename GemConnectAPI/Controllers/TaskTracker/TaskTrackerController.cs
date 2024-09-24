using Task = TaskTrackerConsole.model.Task;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskTrackerConsole.data;
using TaskTrackerConsole.model;
using System.Security.Claims;
using GemConnectAPI.Mappers.TaskTracker;
using GemConnectAPI.DTO.TaskTracker;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        /// <param name="taskManager">The task manager service.</param>
        /// <param name="logger">The logger service.</param>

        public TaskTrackerController(ITaskManager taskManager, ITaskMapper taskMapper,
            ILogger<TaskTrackerController> logger)
        {
            _taskManager = taskManager;
            _taskMapper = taskMapper;
            _logger = logger;
        }
        /// <summary>
        /// Gets all tasks.
        /// </summary>
        /// <returns>A list of tasks.</returns>
        /// <remarks>
        /// Example request:
        ///
        ///     GET /api/TaskTracker/tasks
        ///
        /// </remarks>
        [HttpGet("tasks")]
        public ActionResult<IEnumerable<TaskDto>> GetTasks()
        {
            var tasks = _taskManager.GetTasks();
            //Return the tasks as TaskDto objects instead of Task models
            IEnumerable<TaskDto> enumerable = tasks.Select(_taskMapper.MapTaskToDto);  //pass the method MapTaskToDto to the Select method
            var taskDtos = enumerable;

            return Ok(taskDtos);
        }

        /// <summary>
        /// Gets a specific task by ID.
        /// </summary>
        /// <param name="id">The ID of the task to retrieve.</param>
        /// <returns>The task with the specified ID.</returns>
        /// <response code="200">Returns the task with the specified ID.</response>
        /// <response code="404">If the task is not found.</response>
        /// <remarks>
        /// Example request:
        ///
        ///     GET /api/TaskTracker/task/1
        ///
        /// </remarks>
        [HttpGet("task/{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _taskManager.GetTask(id);

            if (task == null)
            {
                return HandleTaskNotFound(id);
            }

            TaskDto taskDto = _taskMapper.MapTaskToDto(task);

            return Ok(taskDto);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="taskDto">The task data transfer object.</param>
        /// <returns>The newly created task.</returns>
        /// <response code="201">Returns the newly created task.</response>
        /// <response code="400">If the task is invalid.</response>
        /// <remarks>
        /// Example request:
        ///
        ///     POST /api/TaskTracker
        ///     {
        ///        "description": "New Task",
        ///        "status": "TODO"
        ///     }
        ///
        /// </remarks>
        /// // <summary>
        /// Creates a new task (User-level access).
        /// </summary>
        [Authorize(Policy = "USER")]  // Allow users to create tasks
        [HttpPost]
        public IActionResult AddTask([FromBody] TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Parse and validate the status here


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  // Get the User Id from the token
            //Validate and parse the status
            var (statusEnum, errorResponse) = ValidateAndParseStatus(taskDto.Status);
            if (errorResponse != null) { return errorResponse; }

            // Map to Task using the mapper, now passing the already validated statusEnum
            Task newTask = _taskMapper.MaptoTask(taskDto, userId, statusEnum.Value);

            // Add the new task
            _taskManager.AddTask(newTask);
            // Return 201 Created response with the new task's URI
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id });
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="taskDto">The updated task data.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="200">Returns the updated task.</response>
        /// <response code="404">If the task is not found.</response>
        /// <response code="400">If the task data is invalid.</response>
        /// <remarks>
        /// Example request:
        ///
        ///     PUT /api/TaskTracker/1
        ///     {
        ///        "description": "Updated Task",
        ///        "status": "COMPLETE"
        ///     }
        ///
        /// </remarks>
        // PUT api/<TaskTrackerController>/5
        [Authorize(Policy = "USER")]
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskDto taskDto)
        {
            var task = _taskManager.GetTask(id);

            if (task == null)
            {
                return HandleTaskNotFound(id);
            }
            // Try to parse the string status to the Status 
            var (statusEnum, errorResponse) = ValidateAndParseStatus(taskDto.Status);
            if (errorResponse != null) { return errorResponse; }

            task.Description = taskDto.Description;
            task.Status = (Status)statusEnum;
            task.UpdatedAt = DateTime.UtcNow; //Update the timestamp

            //Update task
            _taskManager.UpdateTask(id, task);
            // Use the helper method to map to TaskDto
            var updatedTaskDto = _taskMapper.MapTaskToDto(task);
            return Ok(updatedTaskDto);
        }

        /// <summary>
        /// Deletes a task by ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the task is successfully deleted.</response>
        /// <response code="404">If the task is not found.</response>
        /// <remarks>
        /// Example request:
        ///
        ///     DELETE /api/TaskTracker/1
        ///
        /// </remarks>
        // DELETE api/<TaskTrackerController>/5
        [Authorize(Policy = "USER")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _taskManager.GetTask(id);
            if (task == null)
            {
                return HandleTaskNotFound(id);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  // Get the User Id from the token

            // Ensure users can only delete their own tasks or if they are Admins
            if (task.CreatedBy != userId && !User.IsInRole("ADMIN"))
            {
                return Forbid("You are not allowed to delete this task.");
            }

            _taskManager.DeleteTask(id);
            return NoContent();
        }

        #region Helpers
        private (Status?, IActionResult) ValidateAndParseStatus(string statusString)
        {
            // Attempt to parse the status string to the enum
            if (Enum.TryParse(statusString, true, out Status statusEnum))
            {
                // Parsing successful, return the statusEnum and no error response
                return (statusEnum, null);
            }

            // Log the error and return a BadRequest response with the error message
            _logger.LogError($"Invalid status value '{statusString}' provided.");
            return (null, BadRequest("Invalid status value. Please provide a valid status like 'TODO', 'PENDING', or 'COMPLETE'."));
        }
        private NotFoundObjectResult HandleTaskNotFound(int id)
        {
            // Use a logger for better logging management (assumes ILogger<TaskTrackerController> is injected)
            _logger.LogWarning(message: $"Task with ID {id} could not be found.");

            // Return a 404 Not Found response
            return NotFound($"Task with ID {id} could not be found.");
        }
        #endregion
    }
}
