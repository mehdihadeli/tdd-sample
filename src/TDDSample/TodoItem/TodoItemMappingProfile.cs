using AutoMapper;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.TodoItem;

public class TodoItemMappingProfile : Profile
{
    public TodoItemMappingProfile()
    {
        CreateMap<Models.TodoItem, TodoItemDto>();
    }
}
