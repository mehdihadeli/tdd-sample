using Ardalis.Result;
using MediatR;
using TDDSample.TodoItem.Dtos;
using TDDSample.Users.Dtos;

namespace TDDSample.Users.GetUsers;

public record GetUsers(PageRequest PageRequest) : IRequest<Result<PagedList<UserDto>>>;

internal class GetUsersHandler : IRequestHandler<GetUsers, Result<PagedList<UserDto>>>
{
    public Task<Result<PagedList<UserDto>>> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
