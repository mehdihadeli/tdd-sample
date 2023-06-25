using MediatR;
using OneOf;
using TDDSample.Shared.Data.Repository;
using TDDSample.Shared.Exceptions;

namespace TDDSample.TodoItem.Features.CreatingTodoItem;

public record CreateTodoItem(string Title, bool IsCompleted, int UserId) : IRequest<OneOf<int, BadRequestException>>
{
	public DateTime CreatedOn { get; } = DateTime.Now;
};

public class CreateTodoItemHandler : IRequestHandler<CreateTodoItem, OneOf<int, BadRequestException>>
{
	private readonly IRepository<Models.TodoItem> _todoItemRepository;

	public CreateTodoItemHandler(IRepository<Models.TodoItem> todoItemRepository)
	{
		_todoItemRepository = todoItemRepository;
	}

	public async Task<OneOf<int, BadRequestException>> Handle(
		CreateTodoItem? request,
		CancellationToken cancellationToken)
	{
		if (request is null)
		{
			return new BadRequestException($"{nameof(request)} is required.");
		}

		var todoItem = new Models.TodoItem
					   {
						   Title = request.Title,
						   IsCompleted = request.IsCompleted,
						   UserId = request.UserId,
						   CreatedOn = request.CreatedOn
					   };

		var res = await _todoItemRepository.AddAsync(todoItem, cancellationToken);
		return res.Id;
	}
}