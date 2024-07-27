using MediatR;
using OneOf;
using TDDSample.Shared.Data.Repository;
using TDDSample.Shared.Exceptions;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.TodoItem.Features.GettingTodoItems;

public record GetTodoItems(PageRequest PageRequest) : IRequest<OneOf<PagedList<TodoItemDto>, BadRequestException>>;

internal class GetTodoItemsHandler : IRequestHandler<GetTodoItems, OneOf<PagedList<TodoItemDto>, BadRequestException>>
{
    private readonly IRepository<Models.TodoItem> _todoItemRepository;

    public GetTodoItemsHandler(IRepository<Models.TodoItem> todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<OneOf<PagedList<TodoItemDto>, BadRequestException>> Handle(
        GetTodoItems? request,
        CancellationToken cancellationToken
    )
    {
        if (request is null)
        {
            return new BadRequestException($"{nameof(request)} is required.");
        }

        var pageResult = await _todoItemRepository.GetByPageAsync(request.PageRequest, cancellationToken);

        var pageListDto = pageResult.To<TodoItemDto>(ti => new TodoItemDto(
            ti.Id,
            ti.Title,
            ti.IsCompleted,
            ti.UserId,
            ti.CreatedOn
        ));

        return pageListDto;
    }
}
