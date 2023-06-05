using Ardalis.Result;
using MediatR;
using TDDSample.Shared.Data.Repository;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.TodoItem.Features.CreatingTodoItem;

public record CreateTodoItem(string Title, bool IsCompleted, int UserId) : IRequest<Result<int>>
{
    public DateTime CreatedOn { get; } = DateTime.Now;
};

public class CreateTodoItemHandler : IRequestHandler<CreateTodoItem, Result<int>>
{
    private readonly IRepository<Models.TodoItem> _todoItemRepository;

    public CreateTodoItemHandler(IRepository<Models.TodoItem> todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<Result<int>> Handle(CreateTodoItem? request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            var errors = new List<ValidationError>
            {
                new() { Identifier = nameof(request), ErrorMessage = $"{nameof(request)} is required." }
            };
            return Result<int>.Invalid(errors);
        }

        var todoItem = new Models.TodoItem
        {
            Title = request.Title,
            IsCompleted = request.IsCompleted,
            UserId = request.UserId,
            CreatedOn = request.CreatedOn
        };

        var res = await _todoItemRepository.AddAsync(todoItem, cancellationToken);

        return Result<int>.Success(res.Id);
    }
}
