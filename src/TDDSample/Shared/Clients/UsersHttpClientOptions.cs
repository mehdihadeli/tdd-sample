namespace TDDSample.Shared.Clients;

public class UsersHttpClientOptions
{
    public string BaseAddress { get; set; } = default!;
    public int Timeout { get; set; } = 30;
    public string UsersEndpoint { get; set; } = default!;
}
