using AutoMapper;
using MediatR;
using OneOf;
using TDDSample.Shared.Data.Repository;
using TDDSample.Shared.Exceptions;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.TodoItem.Features.GettingTodoItemById;

public record GetTodoItemById(int Id) : IRequest<OneOf<TodoItemDto, BadRequestException,NotFoundException>>;

public class GetTodoItemByIdHandler : IRequestHandler<GetTodoItemById, OneOf<TodoItemDto, BadRequestException,NotFoundException>>
{
    private readonly IRepository<Models.TodoItem> _todoItemRepository;
    private readonly IMapper _mapper;

    public GetTodoItemByIdHandler(IRepository<Models.TodoItem> todoItemRepository, IMapper mapper)
    {
        _todoItemRepository = todoItemRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<TodoItemDto, BadRequestException,NotFoundException>> Handle(GetTodoItemById? request, CancellationToken cancellationToken)
    {
        if (request is null)
            return new BadRequestException($"{nameof(request)} is required.");

        var todoItem = await _todoItemRepository.GetByIdAsync(request.Id, cancellationToken);
        if (todoItem is null)
        {
            return new NotFoundException($"Todo item with id {request.Id} not found");
        }

        var dto = _mapper.Map<TodoItemDto>(todoItem);

        return dto;
    }
}
