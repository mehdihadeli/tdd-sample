using AutoMapper;
using MediatR;
using OneOf;
using OneOf.Types;
using TDDSample.Shared.Data.Repository;
using TDDSample.Shared.Exceptions;

namespace TDDSample.TodoItem.Features.UpdatingTodoItem;

public record UpdateTodoItem
	(int Id, string Title, bool IsCompleted, int UserId) : IRequest<
		OneOf<None, BadRequestException, NotFoundException>>;

internal class
	UpdateTodoItemHandler : IRequestHandler<UpdateTodoItem, OneOf<None, BadRequestException, NotFoundException>>
{
	private readonly IRepository<Models.TodoItem> _todoItemRepository;
	private readonly IMapper _mapper;

	public UpdateTodoItemHandler(IRepository<Models.TodoItem> todoItemRepository, IMapper mapper)
	{
		_todoItemRepository = todoItemRepository;
		_mapper = mapper;
	}

	public async Task<OneOf<None, BadRequestException, NotFoundException>> Handle(
		UpdateTodoItem? request,
		CancellationToken cancellationToken)
	{
		if (request is null)
		{
			return new BadRequestException($"{nameof(request)} is required.");
		}

		var todoItem = await _todoItemRepository.GetByIdAsync(request.Id, cancellationToken);
		if (todoItem is null) return new NotFoundException($"TodoItem with id '{request.Id}' not found");

		var updatedTodoItem = _mapper.Map(request, todoItem);

		await _todoItemRepository.UpdateAsync(updatedTodoItem, cancellationToken);

		return new None();
	}
}