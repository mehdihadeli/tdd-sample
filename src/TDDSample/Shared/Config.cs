using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TDDSample.Shared.Extensions.ServiceCollectionsExtensions;
using TDDSample.TodoItem;
using TDDSample.Users;

namespace TDDSample.Shared;

public static class Config
{
    public static IServiceCollection AddTddSampleServices(this IServiceCollection services)
    {
        services.AddInfrastructure();
        services.AddTodoItem();
        services.AddUsers();

        return services;
    }

    public static IEndpointRouteBuilder MapTddSampleEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapTodoItemEndpoints();
        routeBuilder.MapUsersEndpoints();

        return routeBuilder;
    }
}
