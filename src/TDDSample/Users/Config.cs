using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TDDSample.TodoItem.Features.CreatingTodoItem;
using TDDSample.TodoItem.Features.GettingTodoItemById;
using TDDSample.TodoItem.Features.GettingTodoItems;
using TDDSample.Users.GetUsers;

namespace TDDSample.Users;

public static class Config
{
    public static IServiceCollection AddUsers(this IServiceCollection services)
    {
        return services;
    }

    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var todoItems = routeBuilder
            .NewVersionedApi("Users")
            .AddEndpointFilter(
                async (efiContext, next) =>
                {
                    var stopwatch = Stopwatch.StartNew();
                    var result = await next(efiContext);
                    stopwatch.Stop();
                    var elapsed = stopwatch.ElapsedMilliseconds;
                    var response = efiContext.HttpContext.Response;
                    response.Headers.Add("X-Response-Time", $"{elapsed} milliseconds");
                    return result;
                }
            );

        var usersV1 = todoItems.MapGroup("api/v{version:apiVersion}/users").HasApiVersion(1.0);

        usersV1.MapGetUsersEndpoint();

        return routeBuilder;
    }
}
