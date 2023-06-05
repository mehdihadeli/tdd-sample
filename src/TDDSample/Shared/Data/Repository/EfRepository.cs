using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.Shared.Data.Repository;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class
{
    private readonly TodoDbContext _dbContext;

    public EfRepository(TodoDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<T>> GetByPageAsync(
        PageRequest pageRequest,
        CancellationToken cancellationToken = default
    )
    {
        var skipAmount = pageRequest.PageSize * (pageRequest.Page - 1);
        var queryable = _dbContext.Set<T>().AsQueryable();

        var results = queryable.Skip(skipAmount).Take(pageRequest.PageSize);

        var totalNumberOfRecords = await queryable.CountAsync(cancellationToken: cancellationToken);
        var mod = totalNumberOfRecords % pageRequest.PageSize;
        var totalPageCount = (totalNumberOfRecords / pageRequest.PageSize) + (mod == 0 ? 0 : 1);

        return new PagedList<T>
        {
            PageNumber = pageRequest.Page,
            PageSize = pageRequest!.PageSize,
            Results = await results.ToListAsync(cancellationToken: cancellationToken),
            TotalNumberOfPages = totalPageCount,
            TotalNumberOfRecords = totalNumberOfRecords
        };
    }
}
