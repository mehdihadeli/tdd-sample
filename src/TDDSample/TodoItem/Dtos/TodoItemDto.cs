namespace TDDSample.TodoItem.Dtos;

public record TodoItemDto(int Id, string Title, bool IsCompleted, int UserId, DateTime CreatedOn);
