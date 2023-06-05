using Ardalis.Result;
using AutoBogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using TDDSample.TodoItem.Dtos;
using TDDSample.TodoItem.Features.GettingTodoItemById;
using Xunit;

namespace TDDSample.Tests.UnitTests.TodoItems.Features.GettingTodoItemById;

public class GetTodoItemByIdEndpointTests
{
    [Fact]
    public async Task handle_should_returns_ok_status_code()
    {
        var mediatr = Substitute.For<IMediator>();
        var todoItemId = 1;

        var todoItemDto = new AutoFaker<TodoItemDto>().RuleFor(x => x.Id, todoItemId).Generate();
        var todoItemResult = Result<TodoItemDto>.Success(todoItemDto);

        mediatr
            .Send(Arg.Is<GetTodoItemById>(query => query.Id == todoItemId), Arg.Any<CancellationToken>())
            .Returns(todoItemResult);

        var requestParameters = new GetTodoItemByIdEndpoint.GetTodoItemByIdRequestParameters(
            todoItemId,
            mediatr,
            CancellationToken.None
        );

        var result = (await GetTodoItemByIdEndpoint.Handle(requestParameters)).Result;

        result.Should().BeOfType<Ok<TodoItemDto>>();
        var okResult = result.As<Ok<TodoItemDto>>();
        okResult.Should().NotBeNull();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task handle_should_returns_correct_todo_item()
    {
        var mediatr = Substitute.For<IMediator>();
        var todoItemId = 1;

        var todoItemDto = new AutoFaker<TodoItemDto>().RuleFor(x => x.Id, todoItemId).Generate();
        var todoItemResult = Result<TodoItemDto>.Success(todoItemDto);

        mediatr
            .Send(Arg.Is<GetTodoItemById>(query => query.Id == todoItemId), Arg.Any<CancellationToken>())
            .Returns(todoItemResult);

        var requestParameters = new GetTodoItemByIdEndpoint.GetTodoItemByIdRequestParameters(
            todoItemId,
            mediatr,
            CancellationToken.None
        );

        var result = (await GetTodoItemByIdEndpoint.Handle(requestParameters)).Result;
        result.Should().BeOfType<Ok<TodoItemDto>>();
        var okResult = result.As<Ok<TodoItemDto>>();
        okResult.Should().NotBeNull();

        okResult.Value.Should().Be(todoItemDto);
    }

    [Fact]
    public async Task handle_should_call_mediator_service_with_correct_parameters_once()
    {
        var mediatr = Substitute.For<IMediator>();
        var todoItemId = 1;

        var todoItemDto = new AutoFaker<TodoItemDto>().RuleFor(x => x.Id, todoItemId).Generate();
        var todoItemResult = Result<TodoItemDto>.Success(todoItemDto);

        mediatr
            .Send(Arg.Is<GetTodoItemById>(query => query.Id == todoItemId), Arg.Any<CancellationToken>())
            .Returns(todoItemResult);

        var requestParameters = new GetTodoItemByIdEndpoint.GetTodoItemByIdRequestParameters(
            todoItemId,
            mediatr,
            CancellationToken.None
        );

        await GetTodoItemByIdEndpoint.Handle(requestParameters);

        await mediatr
            .Received(1)
            .Send(Arg.Is<GetTodoItemById>(query => query.Id == todoItemId), Arg.Any<CancellationToken>());
    }
}
