using AutoBogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using TDDSample.TodoItem.Features.CreatingTodoItem;
using Xunit;

namespace TDDSample.Tests.UnitTests.TodoItems.Features.CreatingTodoItem;

// ref: https://dotnetthoughts.net/unittest-aspnetcore-minimal-apis/
public class CreateTodoItemEndpointTests
{
    [Fact]
    public async Task handle_should_returns_created_status_with_location()
    {
        var request = new AutoFaker<CreateTodoItemEndpoint.CreateTodoItemRequest>().Generate();
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var createdId = 1;

        mediator
            .Send(Arg.Is<CreateTodoItem>(command => command.Title == request.Title), Arg.Any<CancellationToken>())
            .Returns(createdId);

        var requestParameters = new CreateTodoItemEndpoint.CreateTodoItemRequestParameters(
            request,
            mediator,
            cancellationToken
        );

        var todoItemResult = (await CreateTodoItemEndpoint.Handle(requestParameters)).Result;

        todoItemResult.Should().BeOfType<Created<int>>();
        var createdResult = todoItemResult.As<Created<int>>();
        createdResult.Should().NotBeNull();

        createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.Location.Should().Be($"api/v1/todo-items/{createdId}");
    }

    [Fact]
    public async Task handle_should_returns_correct_created_id()
    {
        var request = new AutoFaker<CreateTodoItemEndpoint.CreateTodoItemRequest>().Generate();
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var createdId = 1;

        mediator
            .Send(Arg.Is<CreateTodoItem>(command => command.Title == request.Title), Arg.Any<CancellationToken>())
            .Returns(createdId);

        var requestParameters = new CreateTodoItemEndpoint.CreateTodoItemRequestParameters(
            request,
            mediator,
            cancellationToken
        );

        var todoItemResult = (await CreateTodoItemEndpoint.Handle(requestParameters)).Result;

        todoItemResult.Should().BeOfType<Created<int>>();
        var createdResult = todoItemResult.As<Created<int>>();
        createdResult.Should().NotBeNull();

        createdResult.Value.Should().Be(createdId);
    }

    [Fact]
    public async Task handle_should_call_mediator_service_with_correct_parameters_once()
    {
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var request = new AutoFaker<CreateTodoItemEndpoint.CreateTodoItemRequest>().Generate();
        var createdId = 1;

        var requestParameters = new CreateTodoItemEndpoint.CreateTodoItemRequestParameters(
            request,
            mediator,
            cancellationToken
        );

        mediator
            .Send(Arg.Is<CreateTodoItem>(command => command.Title == request.Title), Arg.Any<CancellationToken>())
            .Returns(createdId);

        await CreateTodoItemEndpoint.Handle(requestParameters);

        await mediator
            .Received(1)
            .Send(Arg.Is<CreateTodoItem>(command => command.Title == request.Title), Arg.Any<CancellationToken>());
    }
}
