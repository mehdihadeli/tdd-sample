using Humanizer;
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
        return routeBuilder
            .MapPut("/{id}", Handle)
            .WithTags(nameof(Models.TodoItem).Pluralize())
            .WithName(nameof(UpdateTodoItem));
    }

    internal static async Task<Results<NoContent, ValidationProblem, ProblemHttpResult>> Handle(
        [AsParameters] UpdateTodoItemRequestParameters requestParameters
    )
    {
        var (id, req, mediator, ct) = requestParameters;

        var command = new UpdateTodoItem(id, req.Title, req.IsCompleted, req.UserId);

        var result = await mediator.Send(command, ct);

        return result.Match<Results<NoContent, ValidationProblem, ProblemHttpResult>>(
            none => TypedResults.NoContent(),
            badRequest =>
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "validation errors", badRequest.ErrorMessages.ToArray() },
                };

                return TypedResults.ValidationProblem(detail: badRequest.Message, errors: errors);
            },
            notFound => TypedResults.Problem(statusCode: StatusCodes.Status404NotFound, detail: notFound.Message)
        );
    }

    internal record UpdateTodoItemRequestParameters(
        int Id,
        UpdateTodoItemRequest Request,
        IMediator Mediator,
        CancellationToken CancellationToken
    );

    public record UpdateTodoItemRequest(string Title, bool IsCompleted, int UserId);
}
