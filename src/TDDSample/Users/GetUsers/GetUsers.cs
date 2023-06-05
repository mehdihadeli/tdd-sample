using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TDDSample.Shared.Clients;
using TDDSample.Shared.Exceptions;
using TDDSample.TodoItem.Dtos;
using TDDSample.Users.Dtos;

namespace TDDSample.Users.GetUsers;

public record GetUsers(PageRequest PageRequest) : IRequest<Result<PagedList<UserDto>>>;

internal class GetUsersHandler : IRequestHandler<GetUsers, Result<PagedList<UserDto>>>
{
    private readonly IUsersHttpClient _usersHttpClient;
    private readonly IMapper _mapper;

    public GetUsersHandler(IUsersHttpClient usersHttpClient, IMapper mapper)
    {
        _usersHttpClient = usersHttpClient;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<UserDto>>> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        try
        {
            var usersList = await _usersHttpClient.GetAllUsersAsync(request.PageRequest, cancellationToken);

            var dtos = usersList.To<UserDto>(u => _mapper.Map<UserDto>(u));

            return Result.Success(dtos);
        }
        catch (HttpResponseException ex)
        {
            return Result.ErrorWithCorrelationId(ex.StatusCode.ToString(), ex.Message);
        }
        catch (Exception e)
        {
            return Result.ErrorWithCorrelationId(StatusCodes.Status500InternalServerError.ToString(), e.Message);
        }
    }
}
