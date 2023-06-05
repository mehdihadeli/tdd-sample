using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TDDSample.Shared.Extensions;
using TDDSample.TodoItem;

namespace TDDSample.Shared;

public static class Config
{
    public static IServiceCollection AddTddSampleServices(this IServiceCollection services)
    {
        services.AddInfrastructure();
        services.AddTodoItem();

        return services;
    }

    public static IEndpointRouteBuilder MapTddSampleEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapTodoItemEndpoints();

        return routeBuilder;
    }
}
