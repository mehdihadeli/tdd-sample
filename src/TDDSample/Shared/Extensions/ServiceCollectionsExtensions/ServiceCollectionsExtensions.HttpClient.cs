using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TDDSample.Shared.Clients;

namespace TDDSample.Shared.Extensions.ServiceCollectionsExtensions;

public partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddUsersHttpClient(this IServiceCollection services)
    {
        services.AddOptions<UsersHttpClientOptions>().BindConfiguration(nameof(UsersHttpClientOptions));

        services
            .AddHttpClient<IUsersHttpClient, UsersHttpClient>()
            .ConfigureHttpClient(
                (sp, httpClient) =>
                {
                    var httpClientOptions = sp.GetRequiredService<IOptions<UsersHttpClientOptions>>().Value;
                    httpClient.BaseAddress = new Uri(httpClientOptions.BaseAddress);
                    httpClient.Timeout = TimeSpan.FromSeconds(httpClientOptions.Timeout);
                }
            );

        return services;
    }
}
