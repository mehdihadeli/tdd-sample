using AutoBogus;
using FluentAssertions;
using NSubstitute;
using TDDSample.Shared.Data.Repository;
using TDDSample.TodoItem.Dtos;
using TDDSample.TodoItem.Features.GettingTodoItems;
using TDDSample.Users.GetUsers;
using TDDSample.Users.Models;
using Xunit;

namespace TDDSample.Tests.UnitTests.Users.Features.GetUsers;

public class GetUsersTests
{
    [Fact]
    public async Task handle_with_valid_request_should_return_todo_items()
    {
        // // Arrange
        // var userRepository = Substitute.For<IRepository<User>>();
        // var handler = new GetUsersHandler(userRepository);
        // var cancellationToken = CancellationToken.None;
        // var page = 1;
        // var pageSize = 10;
        //
        // var request = new GetTodoItems(new PageRequest(page, pageSize));
        //
        // var todoItems = new AutoFaker<TodoItem.Models.TodoItem>().Generate(10);
        // var pageList = new PagedList<TodoItem.Models.TodoItem>
        // {
        //     PageSize = pageSize,
        //     PageNumber = page,
        //     Results = todoItems
        // };
        //
        // userRepository
        //     .GetByPageAsync(Arg.Is<PageRequest>(x => x.Page == page && x.PageSize == pageSize), cancellationToken)
        //     .Returns(pageList);
        //
        // // Act
        // var result = await handler.Handle(request, cancellationToken);
        //
        // // Assert
        // result.Should().NotBeNull();
        // result.IsSuccess.Should().BeTrue();
        // result.Value.Should().BeOfType<PagedList<TodoItemDto>>();
        // var pageListDto = result.Value.As<PagedList<TodoItemDto>>();
        // pageListDto.Should().BeEquivalentTo(pageList, c => c.ExcludingMissingMembers());
    }
}
