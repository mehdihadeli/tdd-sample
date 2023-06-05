using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TDDSample.Shared.Data;
using TDDSample.Shared.Data.Repository;

namespace TDDSample.Shared.Extensions.ServiceCollectionsExtensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddDbContextFactory<TodoDbContext>(
            options => options.UseInMemoryDatabase($"MinimalApiDb-{Guid.NewGuid()}")
        );

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

        services.AddUsersHttpClient();

        return services;
    }
}
