using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace TDDSample.TodoItem.Features.CreatingTodoItem;

internal static class CreateTodoItemEndpoint
{
    internal static RouteHandlerBuilder MapCreateTodoItemEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder
            .MapPost("/", Handle)
            .WithName(nameof(CreateTodoItem))
            .WithTags(nameof(Models.TodoItem).Pluralize());
    }

    internal static async Task<Results<Created<int>, ValidationProblem>> Handle(
        [AsParameters] CreateTodoItemRequestParameters requestParameters
    )
    {
        var (req, mediator, ct) = requestParameters;

        var command = new CreateTodoItem(req.Title, req.IsCompleted, req.UserId);

        var result = await mediator.Send(command, ct);

        return result.Match<Results<Created<int>, ValidationProblem>>(
            id => TypedResults.Created($"api/v1/todo-items/{id}", id),
            badRequest =>
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "validation errors", badRequest.ErrorMessages.ToArray() },
                };

                return TypedResults.ValidationProblem(detail: badRequest.Message, errors: errors);
            }
        );
    }

    internal record CreateTodoItemRequestParameters(
        CreateTodoItemRequest Request,
        IMediator Mediator,
        CancellationToken CancellationToken
    );

    public record CreateTodoItemRequest(string Title, bool IsCompleted, int UserId);
}
