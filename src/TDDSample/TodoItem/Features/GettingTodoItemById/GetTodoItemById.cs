using Ardalis.Result;
using AutoMapper;
using MediatR;
using TDDSample.Shared.Data.Repository;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.TodoItem.Features.GettingTodoItemById;

public record GetTodoItemById(int Id) : IRequest<Result<TodoItemDto>>;

public class GetTodoItemByIdHandler : IRequestHandler<GetTodoItemById, Result<TodoItemDto>>
{
    private readonly IRepository<Models.TodoItem> _todoItemRepository;
    private readonly IMapper _mapper;

    public GetTodoItemByIdHandler(IRepository<Models.TodoItem> todoItemRepository, IMapper mapper)
    {
        _todoItemRepository = todoItemRepository;
        _mapper = mapper;
    }

    public async Task<Result<TodoItemDto>> Handle(GetTodoItemById? request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            var errors = new List<ValidationError>
            {
                new() { Identifier = nameof(request), ErrorMessage = $"{nameof(request)} is required." }
            };
            return Result.Invalid(errors);
        }

        var todoItem = await _todoItemRepository.GetByIdAsync(request.Id, cancellationToken);
        if (todoItem is null)
        {
            return Result.NotFound();
        }

        var dto = _mapper.Map<TodoItemDto>(todoItem);

        return Result.Success(dto);
    }
}
