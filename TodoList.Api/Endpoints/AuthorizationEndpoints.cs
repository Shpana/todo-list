using Carter;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Database;
using TodoList.Api.Repositories;
using TodoList.Api.Requests;
using TodoList.Api.Responses;
using TodoList.Api.Services;

namespace TodoList.Api.Endpoints;

public class AuthorizationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("auth/v1");
        group.MapPost("register", ProcessRegistration);
        group.MapPost("login", ProcessLogin);
    }

    private async Task<IResult> ProcessRegistration(
        [FromBody] ProcessRegisterRequest request,
        IDbConnectionFactory factory,
        UsersRepository usersRepository,
        UserSessionsRepository userSessionsRepository,
        AuthorizationTokenGeneratorService authTokenGenerator)
    {
        using var connection = await factory.CreateConnectionAsync();
        
        {
            var user = await usersRepository.GetUserByEmail(connection, request.Email);
            if (user != null)
                return TypedResults.Conflict(
                    new { Message = $"User with email {request.Email} already exists!" });
        }
        {
            await usersRepository.CreateUser(connection,
                request.Name, request.Email, request.Password);
            
            var authToken = authTokenGenerator.Generate();
            var user = await usersRepository.GetUserByEmail(connection, request.Email);
            await userSessionsRepository.CreateSession(connection, user.Id, authToken);

            return TypedResults.Ok(new ProcessRegisterResponse() { Token = authToken });
        }
    }

    private async Task<IResult> ProcessLogin(
        [FromBody] ProcessLoginRequest request,
        IDbConnectionFactory factory,
        UsersRepository usersRepository, 
        UserSessionsRepository userSessionsRepository,
        AuthorizationTokenGeneratorService authTokenGenerator)
    {
        using var connection = await factory.CreateConnectionAsync();

        var user = await usersRepository.GetUserByEmail(connection, request.Email);
        if (user == null)
            return TypedResults.NotFound();
        if (user.Password != request.Password)
            return TypedResults.Unauthorized();

        var authToken = authTokenGenerator.Generate();
        await userSessionsRepository.CreateOrUpdateSession(connection, user.Id, authToken);
        
        return TypedResults.Ok(new ProcessLoginResponse() { Token = authToken });
    }
}