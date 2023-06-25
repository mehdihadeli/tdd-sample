using AutoBogus;
using FluentAssertions;
using NSubstitute;
using OneOf.Types;
using TDDSample.Shared.Data.Repository;
using TDDSample.Tests.UnitTests.Fixtures;
using TDDSample.TodoItem.Features.UpdatingTodoItem;
using Xunit;

namespace TDDSample.Tests.UnitTests.TodoItems.Features.UpdatingTodoItem;

public class UpdateTodoItemHandlerTests : IClassFixture<MappingFixture>
{
    private readonly MappingFixture _mappingFixture;

    public UpdateTodoItemHandlerTests(MappingFixture mappingFixture)
    {
        _mappingFixture = mappingFixture;
    }

    [Fact]
    public async Task handle_should_update_todoItem_and_returns_result()
    {
        // Arrange
        var id = 1;
        var request = new AutoFaker<UpdateTodoItem>().RuleFor(x => x.Id, id).Generate();

        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new UpdateTodoItemHandler(todoItemRepository, _mappingFixture.Mapper);
        var cancellationToken = CancellationToken.None;

        var existingTodoItem = new AutoFaker<TodoItem.Models.TodoItem>().RuleFor(x => x.Id, id).Generate();

        todoItemRepository.GetByIdAsync(Arg.Is<int>(t => t == request.Id), cancellationToken).Returns(existingTodoItem);

        todoItemRepository
            .UpdateAsync(
                Arg.Is<TodoItem.Models.TodoItem>(
                    t =>
                        t.Id == request.Id
                        && t.Title == request.Title
                        && t.IsCompleted == request.IsCompleted
                        && t.UserId == request.UserId
                ),
                cancellationToken
            )
            .Returns(Task.CompletedTask);

        // Act
       var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Value.As<None>().Should().NotBeNull();
        existingTodoItem.Id.Should().Be(request.Id);
        existingTodoItem.Title.Should().Be(request.Title);
        existingTodoItem.UserId.Should().Be(request.UserId);
    }

    [Fact]
    public async Task handle_should_call_update_on_repository_once()
    {
        // Arrange
        var id = 1;
        var request = new AutoFaker<UpdateTodoItem>().RuleFor(x => x.Id, id).Generate();

        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new UpdateTodoItemHandler(todoItemRepository, _mappingFixture.Mapper);
        var cancellationToken = CancellationToken.None;

        var existingTodoItem = new AutoFaker<TodoItem.Models.TodoItem>().RuleFor(x => x.Id, id).Generate();

        todoItemRepository.GetByIdAsync(Arg.Is<int>(t => t == request.Id), cancellationToken).Returns(existingTodoItem);

        todoItemRepository
            .UpdateAsync(
                Arg.Is<TodoItem.Models.TodoItem>(
                    t =>
                        t.Id == request.Id
                        && t.Title == request.Title
                        && t.IsCompleted == request.IsCompleted
                        && t.UserId == request.UserId
                ),
                cancellationToken
            )
            .Returns(Task.CompletedTask);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Value.As<None>().Should().NotBeNull();

        await todoItemRepository
            .Received(1)
            .UpdateAsync(
                Arg.Is<TodoItem.Models.TodoItem>(
                    t =>
                        t.Id == request.Id
                        && t.Title == request.Title
                        && t.IsCompleted == request.IsCompleted
                        && t.UserId == request.UserId
                ),
                cancellationToken
            );
    }

    [Fact]
    public async Task handle_should_call_get_by_id_on_repository_once()
    {
        // Arrange
        var id = 1;
        var request = new AutoFaker<UpdateTodoItem>().RuleFor(x => x.Id, id).Generate();

        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new UpdateTodoItemHandler(todoItemRepository, _mappingFixture.Mapper);
        var cancellationToken = CancellationToken.None;

        var existingTodoItem = new AutoFaker<TodoItem.Models.TodoItem>().RuleFor(x => x.Id, id).Generate();

        todoItemRepository.GetByIdAsync(Arg.Is<int>(t => t == request.Id), cancellationToken).Returns(existingTodoItem);

        todoItemRepository
            .UpdateAsync(
                Arg.Is<TodoItem.Models.TodoItem>(
                    t =>
                        t.Id == request.Id
                        && t.Title == request.Title
                        && t.IsCompleted == request.IsCompleted
                        && t.UserId == request.UserId
                ),
                cancellationToken
            )
            .Returns(Task.CompletedTask);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Value.As<None>().Should().NotBeNull();

        await todoItemRepository.Received(1).GetByIdAsync(Arg.Is<int>(t => t == request.Id), cancellationToken);
    }
}
