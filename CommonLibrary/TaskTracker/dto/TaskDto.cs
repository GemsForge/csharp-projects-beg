using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.TaskTracker.dto;
/// <summary>
/// The DTO will encapsulate the data you want to send to the UI, often transforming or formatting it as needed.
/// </summary>
public class TaskDto
{
    public int Id { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    public required string Status { get; set; }  // String representation of the status for easier display
    public string CreatedAt { get; set; }  // Formatted date
    public string UpdatedAt { get; set; }  // Formatted date
}
