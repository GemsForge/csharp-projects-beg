using System.ComponentModel.DataAnnotations;

namespace TaskTrackerConsole.dto
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for tasks.
    /// This DTO encapsulates the data to be sent to the UI, often transforming or formatting it as needed.
    /// </summary>
    public class TaskDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDto"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the task.</param>
        /// <param name="description">The task description.</param>
        /// <param name="status">The current status of the task.</param>
        /// <param name="createdAt">The task's creation timestamp in UTC.</param>
        /// <param name="updatedAt">The task's last update timestamp in UTC.</param>
        /// <param name="createdBy">The user who created the task.</param>
        public TaskDto(int id, string description, string status, string createdAt, string updatedAt, string createdBy)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException($"'{nameof(description)}' cannot be null or empty.", nameof(description));
            }

            if (string.IsNullOrEmpty(status))
            {
                throw new ArgumentException($"'{nameof(status)}' cannot be null or empty.", nameof(status));
            }

            if (string.IsNullOrEmpty(createdAt))
            {
                throw new ArgumentException($"'{nameof(createdAt)}' cannot be null or empty.", nameof(createdAt));
            }

            if (string.IsNullOrEmpty(updatedAt))
            {
                throw new ArgumentException($"'{nameof(updatedAt)}' cannot be null or empty.", nameof(updatedAt));
            }

            if (string.IsNullOrWhiteSpace(createdBy))
            {
                throw new ArgumentException($"'{nameof(createdBy)}' cannot be null or whitespace.", nameof(createdBy));
            }

            Id = id;
            Description = description;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            CreatedBy = createdBy;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the task.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        /// <example>Implement the new user authentication module</example>
        [Required]
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the status of the task.
        /// Allowed values are: TODO, PENDING, COMPLETE.
        /// </summary>
        /// <example>TODO</example>
        [Required]
        public required string Status { get; set; }  // String representation of the status for easier display

        /// <summary>
        /// Gets or sets the creation date and time of the task in UTC, formatted as a string.
        /// </summary>
        /// <example>2024-09-10 14:23:45</example>
        public string? CreatedAt { get; set; }  // Formatted date

        /// <summary>
        /// Gets or sets the last updated date and time of the task in UTC, formatted as a string.
        /// </summary>
        /// <example>2024-09-12 09:15:30</example>
        public string? UpdatedAt { get; set; }  // Formatted date

        /// <summary>
        /// Gets or sets the ID of the user who created the task.
        /// </summary>
        /// <example>ADMIN</example>
        public string? CreatedBy { get; internal set; }
    }
}
