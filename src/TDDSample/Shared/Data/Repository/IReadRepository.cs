using Ardalis.Specification;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.Shared.Data.Repository;

public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class
{
    Task<PagedList<T>> GetByPageAsync(PageRequest pageRequest, CancellationToken cancellationToken = default);
}
