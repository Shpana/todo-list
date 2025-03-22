using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Database;
using TodoList.Api.Repositories;
using TodoList.Api.Requests;
using TodoList.Api.Responses;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers;

[Route("auth/v1")]
[ApiController]
public class AuthController(
    IDbConnectionFactory factory,
    IUsersRepository usersRepository,
    IUserSessionsRepository sessionsRepository,
    AuthTokenGeneratorService authTokenGenerator)
    : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> ProcessRegistration([FromBody] ProcessRegisterRequest request)
    {
        using var connection = await factory.CreateConnectionAsync();
        
        {
            var user = await usersRepository.GetUserByEmail(connection, request.Email);
            if (user != null)
                return Conflict(
                    new { Message = $"User with email {request.Email} already exists!" });
        }
        {
            await usersRepository.CreateUser(connection,
                request.Name, request.Email, request.Password);
            
            var authToken = authTokenGenerator.Generate();
            var user = await usersRepository.GetUserByEmail(connection, request.Email);
            await sessionsRepository.CreateSession(connection, user.Id, authToken);

            return Ok(new ProcessRegisterResponse() { Token = authToken });
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> ProcessLogin([FromBody] ProcessLoginRequest request)
    {
        using var connection = await factory.CreateConnectionAsync();

        var user = await usersRepository.GetUserByEmail(connection, request.Email);
        if (user == null)
            return NotFound();
        if (user.Password != request.Password)
            return Unauthorized();

        var authToken = authTokenGenerator.Generate();
        await sessionsRepository.CreateOrUpdateSession(connection, user.Id, authToken);

        return Ok(new ProcessLoginResponse() { Token = authToken });
    }
}