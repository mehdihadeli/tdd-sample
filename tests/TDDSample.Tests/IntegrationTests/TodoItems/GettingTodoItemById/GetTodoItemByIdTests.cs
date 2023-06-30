using AutoBogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TDDSample.Api;
using TDDSample.Shared.Data;
using TDDSample.TodoItem.Dtos;
using TDDSample.TodoItem.Features.GettingTodoItemById;
using Xunit;

namespace TDDSample.Tests.IntegrationTests.TodoItems.GettingTodoItemById;

public class GetTodoItemByIdTests : IClassFixture<WebApplicationFactory<ApiMetadata>>
{
	private readonly HttpClient _httpClient;
	private readonly IServiceProvider _scopedServiceProvider;
	private readonly IMediator _mediator;

	public GetTodoItemByIdTests(WebApplicationFactory<ApiMetadata> factory)
	{
		_httpClient = factory.CreateClient();
		_scopedServiceProvider = factory.Services.CreateScope().ServiceProvider;
		_mediator = _scopedServiceProvider.GetRequiredService<IMediator>();
	}

	[Fact]
	public async Task should_get_todo_item_by_valid_id()
	{
		var todoItem = new AutoFaker<TodoItem.Models.TodoItem>().RuleFor(x => x.Id, 0).Generate();
		var context = _scopedServiceProvider.GetRequiredService<TodoDbContext>();
		await context.TodoItems.AddAsync(todoItem);
		await context.SaveChangesAsync();

		var query = new GetTodoItemById(todoItem.Id);
		var result = await _mediator.Send(query);
		var todoItemDto = result.Value.As<TodoItemDto>();
		todoItemDto.Should().NotBeNull();
		todoItemDto.Should().BeEquivalentTo(todoItem);
	}
}