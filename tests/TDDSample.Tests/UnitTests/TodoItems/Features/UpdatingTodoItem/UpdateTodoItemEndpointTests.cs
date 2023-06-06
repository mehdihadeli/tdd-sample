using Ardalis.Result;
using AutoBogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using TDDSample.TodoItem.Features.UpdatingTodoItem;
using Xunit;

namespace TDDSample.Tests.UnitTests.TodoItems.Features.UpdatingTodoItem;

// ref: https://dotnetthoughts.net/unittest-aspnetcore-minimal-apis/
public class UpdateTodoItemEndpointTests
{
    [Fact]
    public async Task handle_should_returns_nocontent_status_code()
    {
        var request = new AutoFaker<UpdateTodoItemEndpoint.UpdateTodoItemRequest>().Generate();
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var todoItemId = 1;

        var result = Result.Success();
        mediator
            .Send(
                Arg.Is<UpdateTodoItem>(
                    command =>
                        command.Id == todoItemId && command.Title == request.Title && command.UserId == request.UserId
                ),
                Arg.Any<CancellationToken>()
            )
            .Returns(result);

        var requestParameters = new UpdateTodoItemEndpoint.UpdateTodoItemRequestParameters(
            todoItemId,
            request,
            mediator,
            cancellationToken
        );

        var todoItemResult = (await UpdateTodoItemEndpoint.Handle(requestParameters)).Result;

        todoItemResult.Should().BeOfType<NoContent>();
        var noContentResult = todoItemResult.As<NoContent>();
        noContentResult.Should().NotBeNull();

        noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task handle_should_returns_notfound_status_code_when_item_not_exists()
    {
        var request = new AutoFaker<UpdateTodoItemEndpoint.UpdateTodoItemRequest>().Generate();
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var todoItemId = 1;

        var result = Result.NotFound();
        mediator
            .Send(
                Arg.Is<UpdateTodoItem>(
                    command =>
                        command.Id == todoItemId && command.Title == request.Title && command.UserId == request.UserId
                ),
                Arg.Any<CancellationToken>()
            )
            .Returns(result);

        var requestParameters = new UpdateTodoItemEndpoint.UpdateTodoItemRequestParameters(
            todoItemId,
            request,
            mediator,
            cancellationToken
        );

        var todoItemResult = (await UpdateTodoItemEndpoint.Handle(requestParameters)).Result;

        todoItemResult.Should().BeOfType<ProblemHttpResult>();
        var noContentResult = todoItemResult.As<ProblemHttpResult>();
        noContentResult.Should().NotBeNull();

        noContentResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task handle_should_call_mediator_service_with_correct_parameters_once()
    {
        // Arrange
        var request = new AutoFaker<UpdateTodoItemEndpoint.UpdateTodoItemRequest>().Generate();
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var todoItemId = 1;

        var result = Result.Success();
        mediator
            .Send(
                Arg.Is<UpdateTodoItem>(
                    command =>
                        command.Id == todoItemId && command.Title == request.Title && command.UserId == request.UserId
                ),
                Arg.Any<CancellationToken>()
            )
            .Returns(result);

        var requestParameters = new UpdateTodoItemEndpoint.UpdateTodoItemRequestParameters(
            todoItemId,
            request,
            mediator,
            cancellationToken
        );

        // Act
        await UpdateTodoItemEndpoint.Handle(requestParameters);

        // Assert
        await mediator
            .Received(1)
            .Send(
                Arg.Is<UpdateTodoItem>(
                    command =>
                        command.Id == todoItemId && command.Title == request.Title && command.UserId == request.UserId
                ),
                Arg.Any<CancellationToken>()
            );
    }
}
