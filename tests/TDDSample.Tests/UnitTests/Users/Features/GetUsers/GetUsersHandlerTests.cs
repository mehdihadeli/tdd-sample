using Ardalis.Result;
using AutoBogus;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TDDSample.Shared.Clients;
using TDDSample.Shared.Exceptions;
using TDDSample.Tests.UnitTests.Fixtures;
using TDDSample.TodoItem.Dtos;
using TDDSample.Users.Dtos;
using TDDSample.Users.GetUsers;
using TDDSample.Users.Models;
using Xunit;

namespace TDDSample.Tests.UnitTests.Users.Features.GetUsers;

// https://www.testwithspring.com/lesson/the-best-practices-of-nested-unit-tests/

public class GetUsersHandlerTests : IClassFixture<MappingFixture>
{
    private readonly MappingFixture _mappingFixture;

    public GetUsersHandlerTests(MappingFixture mappingFixture)
    {
        _mappingFixture = mappingFixture;
    }

    [Fact]
    public async Task handle_should_call_users_http_client_once()
    {
        var usersHttpClient = Substitute.For<IUsersHttpClient>();
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;

        var users = new AutoFaker<User>().Generate(20);
        var pageResult = new PagedList<User>
        {
            Results = users,
            PageNumber = page,
            PageSize = pageSize,
            TotalNumberOfRecords = users.Count
        };

        usersHttpClient.GetAllUsersAsync(Arg.Any<PageRequest>(), cancellationToken).Returns(pageResult);

        var handler = new GetUsersHandler(usersHttpClient, _mappingFixture.Mapper);

        var query = new TDDSample.Users.GetUsers.GetUsers(new PageRequest(page, pageSize));
        await handler.Handle(query, cancellationToken);

        await usersHttpClient.Received(1).GetAllUsersAsync(Arg.Any<PageRequest>(), cancellationToken);
    }

    [Fact]
    public async Task handle_with_valid_request_should_returns_users()
    {
        var usersHttpClient = Substitute.For<IUsersHttpClient>();
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;

        var users = new AutoFaker<User>().Generate(20);
        var pageResult = new PagedList<User>
        {
            Results = users,
            PageNumber = page,
            PageSize = pageSize,
            TotalNumberOfRecords = users.Count
        };

        usersHttpClient.GetAllUsersAsync(Arg.Any<PageRequest>(), cancellationToken).Returns(pageResult);

        var handler = new GetUsersHandler(usersHttpClient, _mappingFixture.Mapper);

        var query = new TDDSample.Users.GetUsers.GetUsers(new PageRequest(page, pageSize));
        var result = await handler.Handle(query, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<PagedList<UserDto>>();
        var pageListDto = result.Value.As<PagedList<UserDto>>();
        pageListDto.Should().NotBeNull();
        pageListDto.PageNumber.Should().Be(page);
        pageListDto.PageSize.Should().Be(pageSize);
        pageListDto.TotalNumberOfRecords.Should().Be(users.Count);
        pageListDto.Results.Should().BeEquivalentTo(pageResult.Results);
    }

    [Fact]
    public async Task handle_with_http_response_exception_should_returns_correct_error_result()
    {
        var usersHttpClient = Substitute.For<IUsersHttpClient>();
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;

        usersHttpClient
            .GetAllUsersAsync(Arg.Any<PageRequest>(), cancellationToken)
            .ThrowsAsync(new HttpResponseException(404, "Not Found"));

        var handler = new GetUsersHandler(usersHttpClient, _mappingFixture.Mapper);

        var query = new TDDSample.Users.GetUsers.GetUsers(new PageRequest(page, pageSize));
        var result = await handler.Handle(query, cancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.Error);
        result.CorrelationId.Should().Be("404");
        result.Errors.First().Should().Be("Not Found");
    }

    [Fact]
    public async Task handle_with_exception_should_returns_correct_error_result()
    {
        var usersHttpClient = Substitute.For<IUsersHttpClient>();
        var cancellationToken = CancellationToken.None;
        var page = 1;
        var pageSize = 10;

        usersHttpClient
            .GetAllUsersAsync(Arg.Any<PageRequest>(), cancellationToken)
            .ThrowsAsync(new Exception("Internal server error"));

        var handler = new GetUsersHandler(usersHttpClient, _mappingFixture.Mapper);

        var query = new TDDSample.Users.GetUsers.GetUsers(new PageRequest(page, pageSize));
        var result = await handler.Handle(query, cancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.Error);
        result.CorrelationId.Should().Be("500");
        result.Errors.First().Should().Be("Internal server error");
    }
}
