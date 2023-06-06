using AutoMapper;
using TDDSample.TodoItem.Dtos;
using TDDSample.TodoItem.Features.UpdatingTodoItem;

namespace TDDSample.TodoItem;

public class TodoItemMappingProfile : Profile
{
    public TodoItemMappingProfile()
    {
        CreateMap<Models.TodoItem, TodoItemDto>();
        CreateMap<UpdateTodoItem, Models.TodoItem>();
    }
}
