using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.TaskTracker.dto;

/// <summary>
/// Represents a Data Transfer Object (DTO) for tasks.
/// This DTO encapsulates the data to be sent to the UI, often transforming or formatting it as needed.
/// </summary>
public class TaskDto
{
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
}
