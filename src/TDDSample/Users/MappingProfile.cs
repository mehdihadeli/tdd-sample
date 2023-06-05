using AutoMapper;
using TDDSample.Users.Dtos;

namespace TDDSample.Users;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.User, UserDto>();
        CreateMap<Models.Address, AddressDto>();
    }
}
