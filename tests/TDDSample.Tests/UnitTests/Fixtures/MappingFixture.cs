using AutoMapper;
using TDDSample.TodoItem;

namespace TDDSample.Tests.UnitTests.Fixtures;

public class MappingFixture
{
    public MappingFixture()
    {
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        var mapper = configurationProvider.CreateMapper();

        Mapper = mapper;
    }

    public IMapper Mapper { get; }
}
