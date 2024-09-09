using CommonLibrary.TaskTracker.data;
using Task = CommonLibrary.TaskTracker.model.Task;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTrackerController : ControllerBase
    {
        private ITaskRepository _taskRepo;
        private ITaskManager _taskManager;

        public TaskTrackerController()
        {
            string filePath = @"C:\Users\Diamond R. Brown\OneDrive\Gem.Professional 🎖️\02 💻 GemsCode\Git Repositories\CSharpProjects\CommonLibrary\TaskTracker\data\Tasks.json";
            _taskRepo = new TaskRepository(filePath);
            _taskManager = new TaskManager(_taskRepo);
        }
        // GET: api/<TaskTrackerController>
        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return (IEnumerable<Task>)_taskManager.GetTasks();
        }

        // GET api/<TaskTrackerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TaskTrackerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TaskTrackerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaskTrackerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
