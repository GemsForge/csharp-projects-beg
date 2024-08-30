
namespace TaskTracker.model;

public class Task
{
    //id: A unique identifier for the task
    public required int Id { get; set; }
    //description: A short description of the task
    public required string Description { get; set; }
    //status: The status of the task (todo, in-progress, done)
    public Status Status { get; set; }
    //createdAt: The date and time when the task was created
    public DateTime CreatedAt { get; set; }
    //updatedAt: The date and time when the task was last updated
    public DateTime UpdatedAt { get; set; }
}




