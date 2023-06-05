using AutoBogus;
using FluentAssertions;
using NSubstitute;
using TDDSample.Shared.Data.Repository;
using TDDSample.TodoItem.Features.CreatingTodoItem;
using Xunit;

namespace TDDSample.Tests.UnitTests.TodoItems.Features.CreatingTodoItem;

public class CreateTodoItemHandlerTests
{
    [Fact]
    public async Task handle_should_create_todoItem_and_returns_id()
    {
        // Arrange
        var faker = AutoFaker.Create();
        var request = faker.Generate<CreateTodoItem>();

        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new CreateTodoItemHandler(todoItemRepository);
        var cancellationToken = CancellationToken.None;

        var todoItem = new TodoItem.Models.TodoItem
        {
            Title = request.Title,
            IsCompleted = request.IsCompleted,
            UserId = request.UserId,
            Id = 1,
            CreatedOn = request.CreatedOn
        };

        todoItemRepository
            .AddAsync(
                Arg.Is<TodoItem.Models.TodoItem>(
                    t => t.Title == request.Title && t.IsCompleted == request.IsCompleted && t.UserId == request.UserId
                ),
                cancellationToken
            )
            .Returns(todoItem);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Value.Should().Be(todoItem.Id);
    }

    [Fact]
    public async Task handle_should_call_todo_item_repository_with_correct_parameters_once()
    {
        // Arrange
        var faker = AutoFaker.Create();
        var request = faker.Generate<CreateTodoItem>();

        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new CreateTodoItemHandler(todoItemRepository);
        var cancellationToken = CancellationToken.None;

        var todoItem = new TodoItem.Models.TodoItem
        {
            Title = request.Title,
            IsCompleted = request.IsCompleted,
            UserId = request.UserId,
            Id = 1,
            CreatedOn = request.CreatedOn
        };

        todoItemRepository
            .AddAsync(
                Arg.Is<TodoItem.Models.TodoItem>(
                    t => t.Title == request.Title && t.IsCompleted == request.IsCompleted && t.UserId == request.UserId
                ),
                cancellationToken
            )
            .Returns(todoItem);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        await todoItemRepository
            .Received(1)
            .AddAsync(
                Arg.Is<TodoItem.Models.TodoItem>(
                    t => t.Title == request.Title && t.IsCompleted == request.IsCompleted && t.UserId == request.UserId
                ),
                cancellationToken
            );
    }
}
