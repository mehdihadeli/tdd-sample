using Ardalis.Specification;

namespace TDDSample.Shared.Data.Repository;

// from Ardalis.Specification
public interface IRepository<T> : IRepositoryBase<T>, IReadRepository<T>
    where T : class { }
