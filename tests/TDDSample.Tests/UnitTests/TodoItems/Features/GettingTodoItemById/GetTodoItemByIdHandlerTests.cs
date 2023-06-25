using AutoBogus;
using FluentAssertions;
using NSubstitute;
using TDDSample.Shared.Data.Repository;
using TDDSample.Shared.Exceptions;
using TDDSample.Tests.UnitTests.Fixtures;
using TDDSample.TodoItem.Dtos;
using TDDSample.TodoItem.Features.GettingTodoItemById;
using Xunit;

namespace TDDSample.Tests.UnitTests.TodoItems.Features.GettingTodoItemById;

public class GetTodoItemByIdHandlerTests : IClassFixture<MappingFixture>
{
    private readonly MappingFixture _mappingFixture;

    public GetTodoItemByIdHandlerTests(MappingFixture mappingFixture)
    {
        _mappingFixture = mappingFixture;
    }

    [Fact]
    public async Task handle_with_valid_request_should_return_todo_itemDto()
    {
        // Arrange
        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new GetTodoItemByIdHandler(todoItemRepository, _mappingFixture.Mapper);
        var cancellationToken = CancellationToken.None;
        var todoItemId = 123;

        var request = new GetTodoItemById(todoItemId) { Id = todoItemId };

        var todoItem = new AutoFaker<TodoItem.Models.TodoItem>().RuleFor(x => x.Id, todoItemId).Generate();

        todoItemRepository.GetByIdAsync(Arg.Is<int>(x => x == todoItemId), cancellationToken).Returns(todoItem);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        var todoItemDto = result.Value.As<TodoItemDto>();
        todoItemDto.Should().NotBeNull();
        todoItemDto.Should().BeEquivalentTo(todoItem);
    }

    [Fact]
    public async Task handle_should_call_todo_item_repository_with_correct_parameters_once()
    {
        // Arrange
        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new GetTodoItemByIdHandler(todoItemRepository, _mappingFixture.Mapper);
        var cancellationToken = CancellationToken.None;
        var todoItemId = 123;

        var request = new GetTodoItemById(todoItemId) { Id = todoItemId };

        var todoItem = new AutoFaker<TodoItem.Models.TodoItem>().RuleFor(x => x.Id, todoItemId).Generate();

        todoItemRepository.GetByIdAsync(Arg.Is<int>(x => x == todoItemId), cancellationToken).Returns(todoItem);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        await todoItemRepository.Received(1).GetByIdAsync(Arg.Is<int>(x => x == todoItemId), cancellationToken);
    }

    [Fact]
    public async Task handle_with_null_request_should_return_invalid_result()
    {
        // Arrange
        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new GetTodoItemByIdHandler(todoItemRepository, _mappingFixture.Mapper);
        var cancellationToken = CancellationToken.None;

        GetTodoItemById? request = null;

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        var badRequestException = result.Value.As<BadRequestException>();
        badRequestException.Should().NotBeNull();
        badRequestException.Should().NotBeNull();
    }

    [Fact]
    public async Task handle_with_non_existent_todo_item_should_return_not_found_result()
    {
        // Arrange
        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new GetTodoItemByIdHandler(todoItemRepository, _mappingFixture.Mapper);
        var cancellationToken = CancellationToken.None;
        var todoItemId = 123;

        var request = new GetTodoItemById(todoItemId);

        todoItemRepository.GetByIdAsync(Arg.Is(todoItemId), cancellationToken).Returns((TodoItem.Models.TodoItem)null);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        var notfoundException = result.Value.As<NotFoundException>();
        notfoundException.Should().NotBeNull();
    }
}
