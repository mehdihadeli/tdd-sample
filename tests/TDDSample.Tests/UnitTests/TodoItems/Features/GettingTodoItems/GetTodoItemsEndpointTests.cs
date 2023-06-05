using Ardalis.Result;
using AutoBogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using TDDSample.TodoItem.Dtos;
using TDDSample.TodoItem.Features.GettingTodoItems;
using TDDSample.Users.Dtos;
using TDDSample.Users.GetUsers;
using Xunit;

namespace TDDSample.Tests.UnitTests.TodoItems.Features.GettingTodoItems;

public class GetTodoItemsEndpointTests
{
    [Fact]
    public async Task handle_should_return_ok_status_code()
    {
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;
        var requestParameters = new GetTodoItemsEndpoint.GetTodoItemsRequestParameter(
            mediator,
            cancellationToken,
            page,
            pageSize
        );
        var todoItemsDto = new AutoFaker<TodoItemDto>().Generate(20);
        var pageResult = Result.Success(
            new PagedList<TodoItemDto>
            {
                PageSize = pageSize,
                PageNumber = page,
                Results = todoItemsDto
            }
        );

        mediator
            .Send(
                Arg.Is<GetTodoItems>(x => x.PageRequest.Page == page && x.PageRequest.PageSize == pageSize),
                Arg.Any<CancellationToken>()
            )
            .Returns(pageResult);

        var result = (await GetTodoItemsEndpoint.Handle(requestParameters)).Result;

        result.Should().BeOfType<Ok<PagedList<TodoItemDto>>>();
        var okResult = result.As<Ok<PagedList<TodoItemDto>>>();
        okResult.Should().NotBeNull();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task handle_should_call_mediator_service_with_correct_parameters_once()
    {
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;
        var requestParameters = new GetTodoItemsEndpoint.GetTodoItemsRequestParameter(
            mediator,
            cancellationToken,
            page,
            pageSize
        );
        var todoItemsDto = new AutoFaker<TodoItemDto>().Generate(20);
        var pageResult = Result.Success(
            new PagedList<TodoItemDto>
            {
                PageSize = pageSize,
                PageNumber = page,
                Results = todoItemsDto
            }
        );

        mediator
            .Send(
                Arg.Is<GetTodoItems>(x => x.PageRequest.Page == page && x.PageRequest.PageSize == pageSize),
                Arg.Any<CancellationToken>()
            )
            .Returns(pageResult);

        await GetTodoItemsEndpoint.Handle(requestParameters);

        await mediator
            .Received(1)
            .Send(
                Arg.Is<GetTodoItems>(x => x.PageRequest.Page == page && x.PageRequest.PageSize == pageSize),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task handle_should_return_correct_todo_items()
    {
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;
        var requestParameters = new GetTodoItemsEndpoint.GetTodoItemsRequestParameter(
            mediator,
            cancellationToken,
            page,
            pageSize
        );
        var todoItemsDto = new AutoFaker<TodoItemDto>().Generate(20);
        var pageResult = Result.Success(
            new PagedList<TodoItemDto>
            {
                PageSize = pageSize,
                PageNumber = page,
                Results = todoItemsDto
            }
        );

        mediator
            .Send(
                Arg.Is<GetTodoItems>(x => x.PageRequest.Page == page && x.PageRequest.PageSize == pageSize),
                Arg.Any<CancellationToken>()
            )
            .Returns(pageResult);

        var result = (await GetTodoItemsEndpoint.Handle(requestParameters)).Result;

        result.Should().BeOfType<Ok<PagedList<TodoItemDto>>>();
        var okResult = result.As<Ok<PagedList<TodoItemDto>>>();
        okResult.Should().NotBeNull();

        okResult.Value.Should().NotBeNull();
        okResult.Value.PageSize.Should().Be(pageSize);
        okResult.Value.PageNumber.Should().Be(page);
        okResult.Value.Results.Should().HaveCountGreaterThan(0);
        okResult.Value.Should().BeEquivalentTo(pageResult.Value);
    }

    [Fact]
    public async Task handle_with_invalid_result_should_returns_validation_problem()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;
        var requestParameters = new GetTodoItemsEndpoint.GetTodoItemsRequestParameter(
            mediator,
            cancellationToken,
            page,
            pageSize
        );

        var validationErrors = new List<ValidationError>
                               { /* create test validation errors here */
                               };
        var invalidResult = Result<PagedList<TodoItemDto>>.Invalid(validationErrors);

        mediator.Send(Arg.Any<GetTodoItems>(), cancellationToken).Returns(invalidResult);

        // Act
        var actualResult = (await GetTodoItemsEndpoint.Handle(requestParameters)).Result;

        // Assert
        var validationResult = actualResult.Should().BeOfType<ValidationProblem>().Subject;
        validationResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
