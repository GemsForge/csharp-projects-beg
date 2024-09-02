namespace TaskTracker.dto;
/// <summary>
/// The DTO will encapsulate the data you want to send to the UI, often transforming or formatting it as needed.
/// </summary>
public class TaskDto
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; }  // String representation of the status for easier display
    public required string CreatedAt { get; set; }  // Formatted date
    public required string UpdatedAt { get; set; }  // Formatted date
}
