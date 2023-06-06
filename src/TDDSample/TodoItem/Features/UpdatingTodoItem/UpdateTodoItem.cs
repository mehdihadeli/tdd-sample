using Ardalis.Result;
using AutoMapper;
using MediatR;
using TDDSample.Shared.Data.Repository;

namespace TDDSample.TodoItem.Features.UpdatingTodoItem;

public record UpdateTodoItem(int Id, string Title, bool IsCompleted, int UserId) : IRequest<Result>;

internal class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItem, Result>
{
    private readonly IRepository<Models.TodoItem> _todoItemRepository;
    private readonly IMapper _mapper;

    public UpdateTodoItemHandler(IRepository<Models.TodoItem> todoItemRepository, IMapper mapper)
    {
        _todoItemRepository = todoItemRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateTodoItem? request, CancellationToken cancellationToken)
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
            return Result.NotFound();

        var updatedTodoItem = _mapper.Map(request, todoItem);

        await _todoItemRepository.UpdateAsync(updatedTodoItem, cancellationToken);

        return Result.Success();
    }
}
