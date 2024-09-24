using System.Text.Json.Serialization;

namespace TaskTrackerConsole.model
{
    /// <summary>
    /// Represents a task in the task tracker system.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the task description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the status of the task.
        /// </summary>
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time of the task.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time of the task.
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the created by property with the user Id
        /// </summary>
        [JsonPropertyName("created_by")]
        public int CreatedBy { get; set; }
    }
    // Define a wrapper class to match the new JSON structure
    public class TaskWrapper
    {
        [JsonPropertyName("tasks")]
        public List<Task> Tasks { get; set; }
    }
}
