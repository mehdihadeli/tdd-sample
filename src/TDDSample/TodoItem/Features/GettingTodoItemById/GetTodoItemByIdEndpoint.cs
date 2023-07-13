using Humanizer;
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
		return routeBuilder.MapGet("/{id}", Handle)
			.WithTags(nameof(Models.TodoItem).Pluralize())
			.WithName(nameof(GetTodoItemById));
	}

	internal static async Task<Results<Ok<TodoItemDto>, ValidationProblem, ProblemHttpResult>> Handle(
		[AsParameters] GetTodoItemByIdRequestParameters requestParameters)
	{
		var (id, mediator, ct) = requestParameters;

		var query = new GetTodoItemById(id);
		var result = await mediator.Send(query, ct);

		return result.Match<Results<Ok<TodoItemDto>, ValidationProblem, ProblemHttpResult>>(
			todoItem => TypedResults.Ok(todoItem),
			badRequestException =>
			{
				var errors = new Dictionary<string, string[]>
							 {
								 {"validation errors", badRequestException.ErrorMessages.ToArray()},
							 };

				return TypedResults.ValidationProblem(detail: badRequestException.Message, errors: errors);
			},
			notFoundException => TypedResults.Problem(
				statusCode: StatusCodes.Status404NotFound,
				detail: notFoundException.Message));
	}

	internal record GetTodoItemByIdRequestParameters(int Id, IMediator Mediator, CancellationToken CancellationToken);
}