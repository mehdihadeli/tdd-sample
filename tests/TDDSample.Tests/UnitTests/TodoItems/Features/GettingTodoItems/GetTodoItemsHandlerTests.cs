using AutoBogus;
using FluentAssertions;
using NSubstitute;
using TDDSample.Shared.Data.Repository;
using TDDSample.Tests.UnitTests.Fixtures;
using TDDSample.TodoItem.Dtos;
using TDDSample.TodoItem.Features.GettingTodoItems;
using Xunit;

namespace TDDSample.Tests.UnitTests.TodoItems.Features.GettingTodoItems;

public class GetTodoItemsHandlerTests : IClassFixture<MappingFixture>
{
    private readonly MappingFixture _mappingFixture;

    public GetTodoItemsHandlerTests(MappingFixture mappingFixture)
    {
        _mappingFixture = mappingFixture;
    }

    [Fact]
    public async Task handle_with_valid_request_should_return_todo_items()
    {
        // Arrange
        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new GetTodoItemsHandler(todoItemRepository);
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;

        var request = new GetTodoItems(new PageRequest(page, pageSize));

        var todoItems = new AutoFaker<TodoItem.Models.TodoItem>().Generate(10);
        var pageList = new PagedList<TodoItem.Models.TodoItem>
        {
            PageSize = pageSize,
            PageNumber = page,
            Results = todoItems
        };

        todoItemRepository
            .GetByPageAsync(Arg.Is<PageRequest>(x => x.Page == page && x.PageSize == pageSize), cancellationToken)
            .Returns(pageList);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<PagedList<TodoItemDto>>();
        var pageListDto = result.Value.As<PagedList<TodoItemDto>>();
        pageListDto.Should().BeEquivalentTo(pageList, c => c.ExcludingMissingMembers());
    }

    [Fact]
    public async Task handle_should_call_todo_item_repository_with_correct_parameters_once()
    {
        // Arrange
        var todoItemRepository = Substitute.For<IRepository<TodoItem.Models.TodoItem>>();
        var handler = new GetTodoItemsHandler(todoItemRepository);
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;

        var request = new GetTodoItems(new PageRequest(page, pageSize));

        var todoItems = new AutoFaker<TodoItem.Models.TodoItem>().Generate(10);
        var pageList = new PagedList<TodoItem.Models.TodoItem>
        {
            PageSize = pageSize,
            PageNumber = page,
            Results = todoItems
        };

        todoItemRepository
            .GetByPageAsync(Arg.Is<PageRequest>(x => x.Page == page && x.PageSize == pageSize), cancellationToken)
            .Returns(pageList);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        await todoItemRepository
            .Received(1)
            .GetByPageAsync(Arg.Is<PageRequest>(x => x.Page == page && x.PageSize == pageSize), cancellationToken);
    }
}
