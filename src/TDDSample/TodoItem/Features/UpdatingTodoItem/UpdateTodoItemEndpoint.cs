using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace TDDSample.TodoItem.Features.UpdatingTodoItem;

internal static class UpdateTodoItemEndpoint
{
    internal static RouteHandlerBuilder MapUpdateTodoItemEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder.MapPut("/{id}", Handle).WithName(nameof(UpdateTodoItem));
    }

    internal static async Task<Results<NoContent, ValidationProblem, ProblemHttpResult>> Handle(
        [AsParameters] UpdateTodoItemRequestParameters requestParameters
    )
    {
        var (id, req, mediator, ct) = requestParameters;

        var command = new UpdateTodoItem(id, req.Title, req.IsCompleted, req.UserId);

        var result = await mediator.Send(command, ct);

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

        return TypedResults.NoContent();
    }

    internal record UpdateTodoItemRequestParameters(
        int Id,
        UpdateTodoItemRequest Request,
        IMediator Mediator,
        CancellationToken CancellationToken
    );

    public record UpdateTodoItemRequest(string Title, bool IsCompleted, int UserId);
}
