using TDDSample.TodoItem.Dtos;
using TDDSample.Users.Models;

namespace TDDSample.Shared.Clients;

public interface IUsersHttpClient
{
    Task<PagedList<User>> GetAllUsersAsync(PageRequest pageRequest, CancellationToken cancellationToken = default);
}
