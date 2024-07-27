using Humanizer;
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
        return routeBuilder
            .MapGet("/", Handle)
            .WithTags(nameof(Models.TodoItem).Pluralize())
            .WithName(nameof(GetTodoItems));
    }

    internal static async Task<Results<Ok<PagedList<TodoItemDto>>, ValidationProblem>> Handle(
        [AsParameters] GetTodoItemsRequestParameter requestParameters
    )
    {
        var (mediator, cancellationToken, page, pageSize) = requestParameters;
        var query = new GetTodoItems(new PageRequest(page, pageSize));

        var result = await mediator.Send(query, cancellationToken);

        return result.Match<Results<Ok<PagedList<TodoItemDto>>, ValidationProblem>>(
            todoItemList => TypedResults.Ok(todoItemList),
            badRequestException =>
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "validation errors", badRequestException.ErrorMessages.ToArray() },
                };

                return TypedResults.ValidationProblem(detail: badRequestException.Message, errors: errors);
            }
        );
    }

    internal record GetTodoItemsRequestParameter(
        IMediator Mediator,
        CancellationToken CancellationToken,
        int Page = 1,
        int PageSize = 10
    );
}
