using AutoMapper;
using MediatR;
using OneOf;
using TDDSample.Shared.Clients;
using TDDSample.Shared.Exceptions;
using TDDSample.TodoItem.Dtos;
using TDDSample.Users.Dtos;

namespace TDDSample.Users.GetUsers;

public record GetUsers(PageRequest PageRequest)
    : IRequest<OneOf<PagedList<UserDto>, HttpResponseException, InternalServerException>>;

internal class GetUsersHandler
    : IRequestHandler<GetUsers, OneOf<PagedList<UserDto>, HttpResponseException, InternalServerException>>
{
    private readonly IUsersHttpClient _usersHttpClient;
    private readonly IMapper _mapper;

    public GetUsersHandler(IUsersHttpClient usersHttpClient, IMapper mapper)
    {
        _usersHttpClient = usersHttpClient;
        _mapper = mapper;
    }

    public async Task<OneOf<PagedList<UserDto>, HttpResponseException, InternalServerException>> Handle(
        GetUsers request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var usersList = await _usersHttpClient.GetAllUsersAsync(request.PageRequest, cancellationToken);

            var dtos = usersList.To<UserDto>(u => _mapper.Map<UserDto>(u));

            return dtos;
        }
        catch (HttpResponseException ex)
        {
            return ex;
        }
        catch (Exception e)
        {
            return new InternalServerException("internal error", e);
        }
    }
}
