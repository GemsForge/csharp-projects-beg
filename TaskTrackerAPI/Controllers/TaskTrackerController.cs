using CommonLibrary.TaskTracker.data;
using Task = CommonLibrary.TaskTracker.model.Task;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary.TaskTracker.dto;
using CommonLibrary.TaskTracker.model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTrackerController : ControllerBase
    {
        private readonly ITaskManager _taskManager;
        private readonly ILogger<TaskTrackerController> _logger;

        public TaskTrackerController(ITaskManager taskManager, ILogger<TaskTrackerController> logger)
        {
            _taskManager = taskManager;
            _logger = logger;
        }
        // GET: api/<TaskTrackerController>
        [HttpGet("tasks")]
        public ActionResult<IEnumerable<TaskDto>> Get()
        {
            var tasks = _taskManager.GetTasks();
            //Return the tasks as TaskDto objects instead of Task models
            IEnumerable<TaskDto> enumerable = tasks.Select(MapTaskToDto);  //pass the method MapTaskToDto to the Select method
            var taskDtos = enumerable;

            return Ok(taskDtos);
        }

        // GET api/<TaskTrackerController>/5
        [HttpGet("task/{id}")]
        public IActionResult Get(int id)
        {
            var task = _taskManager.GetTask(id);

            if (task == null)
            {
                return HandleTaskNotFound(id);
            }

            var taskDto = MapTaskToDto(task);

            return Ok(taskDto);
        }

        // POST api/<TaskTrackerController>
        [HttpPost]
        public IActionResult Post([FromBody] TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Try to parse the string status to the Status enum
            if (!TryParseStatus(taskDto.Status, out Status statusEnum))  // Corrected to use "!"
            {
                _logger.LogError($"ERROR: Invalid status value '{taskDto.Status}' provided.");
                return BadRequest("Invalid status value. Please provide a valid status like 'TODO', 'PENDING', or 'COMPLETE'.");
            }

            Task newTask = new()
            {
                Description = taskDto.Description,
                Status = statusEnum,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add the new task
            _taskManager.AddTask(newTask);

            // Use the helper method to map to TaskDto
            var newTaskDto = MapTaskToDto(newTask);

            // Return 201 Created response with the new task's URI
            return CreatedAtAction(nameof(Get), new { id = newTask.Id }, newTaskDto);
        }


        // PUT api/<TaskTrackerController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TaskDto taskDto)
        {
            var task = _taskManager.GetTask(id);

            if (task == null)
            {
                return HandleTaskNotFound(id);
            }
            // Try to parse the string status to the Status 
            if (!TryParseStatus(taskDto.Status, out Status statusEnum))
            {
                _logger.LogError(message: $"ERROR: Invalid status value '{taskDto.Status}' provided.");
                return BadRequest("Invalid status value. Please provide a valid status like 'TODO', 'PENDING', or 'COMPLETE'.");
            }
            task.Description = taskDto.Description;
            task.Status = statusEnum;
            task.UpdatedAt = DateTime.UtcNow; //Update the timestamp

            //Update task
            _taskManager.UpdateTask(id, task);
            // Use the helper method to map to TaskDto
            var updatedTaskDto = MapTaskToDto(task);
            return Ok(updatedTaskDto);
        }

        // DELETE api/<TaskTrackerController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _taskManager.GetTask(id);
            if (task == null)
            {
                return HandleTaskNotFound(id);
            }

            //Delete task
            _taskManager.DeleteTask(id);
            return NoContent();

        }

        #region Helpers
        private IActionResult HandleTaskNotFound(int id)
        {
            // Use a logger for better logging management (assumes ILogger<TaskTrackerController> is injected)
            _logger.LogWarning(message: $"Task with ID {id} could not be found.");

            // Return a 404 Not Found response
            return NotFound($"Task with ID {id} could not be found.");
        }
        private bool TryParseStatus(string statusString, out Status status) => Enum.TryParse(statusString, true, out status);
        private TaskDto MapTaskToDto(Task task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Description = task.Description,
                Status = task.Status.ToString(),
                CreatedAt = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdatedAt = task.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
        #endregion
    }
}
