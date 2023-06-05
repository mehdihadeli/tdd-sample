using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TDDSample.TodoItem.Features.CreatingTodoItem;
using TDDSample.TodoItem.Features.GettingTodoItemById;
using TDDSample.TodoItem.Features.GettingTodoItems;

namespace TDDSample.TodoItem;

public static class Config
{
    public static IServiceCollection AddTodoItem(this IServiceCollection services)
    {
        return services;
    }

    public static IEndpointRouteBuilder MapTodoItemEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var todoItems = routeBuilder
            .NewVersionedApi("Todo Items")
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

        var todoItemsV1 = todoItems.MapGroup("api/v{version:apiVersion}/todo-items").HasApiVersion(1.0);

        todoItemsV1.MapCreateTodoItemEndpoint();
        todoItemsV1.MapGetTodoItemByIdEndpoint();
        todoItemsV1.MapGetTodoItemsEndpoint();

        return routeBuilder;
    }
}
