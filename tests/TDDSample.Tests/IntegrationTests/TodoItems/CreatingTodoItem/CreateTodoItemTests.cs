using AutoBogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TDDSample.Api;
using TDDSample.TodoItem.Features.CreatingTodoItem;
using Xunit;

namespace TDDSample.Tests.IntegrationTests.TodoItems.CreatingTodoItem;

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
    public async Task should_create_todo_item_with_valid_input()
    {
        var createTodoItem = new AutoFaker<CreateTodoItem>().Generate();

        var result = await _mediator.Send(createTodoItem);
        result.Should().NotBeNull();
        result.Value.As<int>().Should().BeGreaterThan(0);
    }
}
