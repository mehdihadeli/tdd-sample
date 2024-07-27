using System.Net.Http.Json;
using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using TDDSample.Shared.Clients.Dtos;
using TDDSample.Shared.Extensions;
using TDDSample.TodoItem.Dtos;
using TDDSample.Users.Models;

namespace TDDSample.Shared.Clients;

public class UsersHttpClient : IUsersHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly UsersHttpClientOptions _userHttpClientOptions;

    public UsersHttpClient(
        HttpClient httpClient,
        IMapper mapper,
        IOptions<UsersHttpClientOptions> userHttpClientOptions
    )
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _userHttpClientOptions = userHttpClientOptions.Value;
    }

    public async Task<PagedList<User>> GetAllUsersAsync(
        PageRequest pageRequest,
        CancellationToken cancellationToken = default
    )
    {
        // https://stackoverflow.com/a/67877742/581476
        var qb = new QueryBuilder
        {
            { "limit", pageRequest.PageSize.ToString() },
            { "skip", pageRequest.Page.ToString() },
        };

        // https://github.com/App-vNext/Polly#handing-return-values-and-policytresult
        var httpResponse = await _httpClient.GetAsync(
            $"{_userHttpClientOptions.UsersEndpoint}?{qb.ToQueryString().Value}",
            cancellationToken
        );

        // https://stackoverflow.com/questions/21097730/usage-of-ensuresuccessstatuscode-and-handling-of-httprequestexception-it-throws
        // throw HttpResponseException instead of HttpRequestException (because we want detail response exception) with corresponding status code
        await httpResponse.EnsureSuccessStatusCodeWithDetailAsync();

        var usersListPage = await httpResponse.Content.ReadFromJsonAsync<UsersListPageClientDto>(
            cancellationToken: cancellationToken
        );

        if (usersListPage is null)
            throw new Exception("users page list cannot be null");

        var mod = usersListPage.Total % usersListPage.Limit;
        var totalPageCount = usersListPage.Total / usersListPage.Limit + (mod == 0 ? 0 : 1);

        var items = _mapper.Map<IEnumerable<User>>(usersListPage.Users);

        var pageList = new PagedList<User>
        {
            Results = items,
            PageSize = usersListPage.Limit,
            PageNumber = usersListPage.Skip,
            TotalNumberOfRecords = usersListPage.Total,
            TotalNumberOfPages = totalPageCount
        };

        return pageList;
    }
}
