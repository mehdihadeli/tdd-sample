using Ardalis.Result;
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
        return routeBuilder.MapGet("/", Handle).WithName("Users");
    }

    internal static async Task<Results<Ok<PagedList<UserDto>>, ValidationProblem, ProblemHttpResult>> Handle(
        [AsParameters] GetUsersRequestParameters requestParameters
    )
    {
        var (mediator, cancellationToken, page, pageSize) = requestParameters;
        var query = new GetUsers(new PageRequest(page, pageSize));
        var result = await mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.Invalid)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "validation error", result.ValidationErrors.Select(x => x.ErrorMessage).ToArray() }
            };
            return TypedResults.ValidationProblem(errors: errors);
        }

        if (result.Status == ResultStatus.Error)
        {
            var statusCode = int.Parse(result.CorrelationId);
            return TypedResults.Problem(statusCode: statusCode, detail: result.Errors.FirstOrDefault());
        }

        return TypedResults.Ok(result.Value);
    }

    internal record GetUsersRequestParameters(
        IMediator Mediator,
        CancellationToken CancellationToken,
        int Page = 1,
        int PageSize = 10
    );
}