using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.TodoItem.Features.GettingTodoItems;

public static class GetTodoItemsEndpoint
{
    internal static RouteHandlerBuilder MapGetTodoItemsEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder.MapGet("/", Handle).WithName(nameof(GetTodoItems));
    }

    internal static async Task<Results<Ok<PagedList<TodoItemDto>>, ValidationProblem>> Handle(
        [AsParameters] GetTodoItemsRequestParameter requestParameters
    )
    {
        var (mediator, cancellationToken, page, pageSize) = requestParameters;
        var query = new GetTodoItems(new PageRequest(page, pageSize));

        var result = await mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.Invalid)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "validation errors", result.ValidationErrors.Select(x => x.ErrorMessage).ToArray() },
            };
            return TypedResults.ValidationProblem(errors: errors);
        }
        var dto = result.Value;
        return TypedResults.Ok(dto);
    }

    internal record GetTodoItemsRequestParameter(
        IMediator Mediator,
        CancellationToken CancellationToken,
        int Page = 1,
        int PageSize = 10
    );
}
