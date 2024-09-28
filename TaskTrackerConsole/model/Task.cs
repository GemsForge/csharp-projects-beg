using System.Text.Json.Serialization;

namespace TaskTrackerConsole.model
{
    /// <summary>
    /// Represents a task in the task tracker system.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Gets or sets the unique identifier for the task.
        /// </summary>
        /// <example>1</example>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        /// <example>Complete project documentation</example>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the status of the task.
        /// </summary>
        /// <example>TODO</example>
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time of the task.
        /// </summary>
        /// <example>2024-09-28T14:45:00</example>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time of the task.
        /// </summary>
        /// <example>2024-09-29T10:30:00</example>
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the task.
        /// </summary>
        /// <example>101</example>
        [JsonPropertyName("created_by")]
        public int CreatedBy { get; set; }
    }

    /// <summary>
    /// Wrapper for a list of tasks in the task tracker system.
    /// </summary>
    public class TaskWrapper
    {
        /// <summary>
        /// Gets or sets the list of tasks.
        /// </summary>
        [JsonPropertyName("tasks")]
        public List<Task>? Tasks { get; set; } = new List<Task>();
    }
}
