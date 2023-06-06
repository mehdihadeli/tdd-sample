using AutoBogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TDDSample.Api;
using TDDSample.TodoItem.Features.CreatingTodoItem;
using Xunit;

namespace TDDSample.Tests.EndToEndTests.CreatingTodoItem;

public class CreateTodoItemTests : IClassFixture<WebApplicationFactory<ApiMetadata>>
{
    private readonly HttpClient _httpClient;
    private readonly IServiceProvider _scopedServiceProvider;
    private readonly IMediator _mediator;

    public CreateTodoItemTests(WebApplicationFactory<ApiMetadata> factory)
    {
        _httpClient = factory.CreateClient();
        _scopedServiceProvider = factory.Services.CreateScope().ServiceProvider;
        _mediator = _scopedServiceProvider.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task should_create_todo_item_with_created_status_code()
    {
        // Arrange
        var createTodoItem = new AutoFaker<CreateTodoItem>().Generate();

        var route = "api/v1/todo-items";

        // Act
        var response = await _httpClient.PostAsJsonAsync(route, createTodoItem);

        // Assert
        response.Should().Be201Created();
    }

    [Fact]
    public async Task should_create_todo_item_with_and_return_created_id()
    {
        // Arrange
        var createTodoItem = new AutoFaker<CreateTodoItem>().Generate();

        var route = "api/v1/todo-items";

        // Act
        var response = await _httpClient.PostAsJsonAsync(route, createTodoItem);

        // Assert
        response.Should().Satisfy<int>(x => x.Should().BeGreaterThan(0));
    }
}
