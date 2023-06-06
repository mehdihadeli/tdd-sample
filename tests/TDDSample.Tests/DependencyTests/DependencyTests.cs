using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TDDSample.Api;
using Xunit;

namespace TDDSample.Tests.DependencyTests;

public class DependencyTests : IClassFixture<WebApplicationFactory<ApiMetadata>>
{
    private readonly IServiceProvider _scopedServiceProvider;

    public DependencyTests(WebApplicationFactory<ApiMetadata> factory)
    {
        _scopedServiceProvider = factory.Services.CreateScope().ServiceProvider;
    }

    [Fact]
    public void should_resolve_mediator()
    {
        var mediator = _scopedServiceProvider.GetService<IMediator>();
        mediator.Should().NotBeNull();
    }
}
