namespace TDDSample.TodoItem.Models;

public class TodoItem
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required bool IsCompleted { get; init; }
    public required int UserId { get; init; }
    public required DateTime CreatedOn { get; init; }
}
