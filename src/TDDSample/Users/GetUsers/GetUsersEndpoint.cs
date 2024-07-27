using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using TDDSample.TodoItem.Dtos;
using TDDSample.Users.Dtos;

namespace TDDSample.Users.GetUsers;

internal static class GetUsersEndpoint
{
    internal static RouteHandlerBuilder MapGetUsersEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder
            .MapGet("/", Handle)
            .WithTags(nameof(Models.User).Pluralize())
            .WithName(nameof(GetUsers).Pluralize());
    }

    internal static async Task<Results<Ok<PagedList<UserDto>>, ProblemHttpResult>> Handle(
        [AsParameters] GetUsersRequestParameters requestParameters
    )
    {
        var (mediator, cancellationToken, page, pageSize) = requestParameters;
        var query = new GetUsers(new PageRequest(page, pageSize));
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<Results<Ok<PagedList<UserDto>>, ProblemHttpResult>>(
            usersList => TypedResults.Ok(usersList),
            httpResponseException =>
                TypedResults.Problem(
                    detail: httpResponseException.Message,
                    statusCode: httpResponseException.StatusCode
                ),
            internalException =>
                TypedResults.Problem(
                    detail: internalException.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                )
        );
    }

    internal record GetUsersRequestParameters(
        IMediator Mediator,
        CancellationToken CancellationToken,
        int Page = 1,
        int PageSize = 10
    );
}
