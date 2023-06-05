using AutoMapper;
using TDDSample.TodoItem;
using TDDSample.Users;

namespace TDDSample.Tests.UnitTests.Fixtures;

public class MappingFixture
{
    public MappingFixture()
    {
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TodoItemMappingProfile>();
            cfg.AddProfile<UsersMappingProfile>();
        });

        var mapper = configurationProvider.CreateMapper();

        Mapper = mapper;
    }

    public IMapper Mapper { get; }
}
