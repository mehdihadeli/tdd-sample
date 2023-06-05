using AutoMapper;
using TDDSample.Shared.Clients.Dtos;
using TDDSample.Users.Dtos;
using TDDSample.Users.Models;

namespace TDDSample.Users;

public class UsersMappingProfile : Profile
{
    public UsersMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Address, AddressDto>();

        CreateMap<UserClientDto, User>();
        CreateMap<AddressClientDto, Address>();
    }
}
