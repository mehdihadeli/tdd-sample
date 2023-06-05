using AutoMapper;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.TodoItem;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.TodoItem, TodoItemDto>();
    }
}
