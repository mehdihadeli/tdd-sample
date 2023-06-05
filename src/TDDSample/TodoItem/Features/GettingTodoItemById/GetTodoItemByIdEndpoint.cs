using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using TDDSample.TodoItem.Dtos;

namespace TDDSample.TodoItem.Features.GettingTodoItemById;

public static class GetTodoItemByIdEndpoint
{
    internal static RouteHandlerBuilder MapGetTodoItemByIdEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder.MapGet("/{id}", Handle).WithName(nameof(GetTodoItemById));
    }

    internal static async Task<Results<Ok<TodoItemDto>, ValidationProblem, ProblemHttpResult>> Handle(
        [AsParameters] GetTodoItemByIdRequestParameters requestParameters
    )
    {
        var (id, mediator, ct) = requestParameters;

        var query = new GetTodoItemById(id);
        var result = await mediator.Send(query, ct);

        if (result.Status == ResultStatus.Invalid)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "validation errors", result.ValidationErrors.Select(x => x.ErrorMessage).ToArray() },
            };
            return TypedResults.ValidationProblem(errors: errors);
        }

        if (result.Status == ResultStatus.NotFound)
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status404NotFound);
        }

        return TypedResults.Ok(result.Value);
    }

    internal record GetTodoItemByIdRequestParameters(int Id, IMediator Mediator, CancellationToken CancellationToken);
}
