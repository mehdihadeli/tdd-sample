using Ardalis.Result;
using MediatR;
using TDDSample.Shared.Data.Repository;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.TodoItem.Features.GettingTodoItems;

public record GetTodoItems(PageRequest PageRequest) : IRequest<Result<PagedList<TodoItemDto>>>;

internal class GetTodoItemsHandler : IRequestHandler<GetTodoItems, Result<PagedList<TodoItemDto>>>
{
    private readonly IRepository<Models.TodoItem> _todoItemRepository;

    public GetTodoItemsHandler(IRepository<Models.TodoItem> todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<Result<PagedList<TodoItemDto>>> Handle(
        GetTodoItems? request,
        CancellationToken cancellationToken
    )
    {
        if (request is null)
        {
            var errors = new List<ValidationError>
            {
                new() { Identifier = nameof(request), ErrorMessage = $"{nameof(request)} is required." }
            };
            return Result.Invalid(errors);
        }

        var pageResult = await _todoItemRepository.GetByPageAsync(request.PageRequest, cancellationToken);

        var pageResultDto = pageResult.To<TodoItemDto>(
            ti => new TodoItemDto(ti.Id, ti.Title, ti.IsCompleted, ti.UserId, ti.CreatedOn)
        );

        return Result.Success(pageResultDto);
    }
}
