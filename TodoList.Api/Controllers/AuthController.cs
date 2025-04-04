﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TodoList.Api;

[Route("auth/v1")]
[ApiController]
public class AuthController(
    ILogger<AuthController> logger,
    IDbConnectionFactory factory,
    IUsersRepository usersRepository,
    IUserLoginSessionsRepository sessionsRepository,
    PasswordHasher<UserHashView> hasher,
    AuthTokenGeneratorService authTokenGenerator)
    : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> ProcessRegister([FromBody] AuthV1RegisterRequest request)
    {
        using var connection = await factory.CreateConnectionAsync();
        
        if (await usersRepository.HasUserWithEmail(connection, request.Email))
            return Conflict(
                new { Message = $"User with email {request.Email} already exists!" });

        var user = await usersRepository.CreateUser(
            connection,
            request.Name, request.Email, 
            hasher.HashPassword(new UserHashView { Name = request.Name, Email = request.Email }, request.Password));
        var authToken = authTokenGenerator.Generate();
        var session = await sessionsRepository.CreateOrUpdateSession(connection, user.Id, authToken);
        
        return Ok(session.ToAuthV1RegisterResponse());
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> ProcessLogin([FromBody] AuthV1LoginRequest request)
    {
        using var connection = await factory.CreateConnectionAsync();

        var user = await usersRepository.GetUserByEmail(connection, request.Email);
        if (user == null)
            return NotFound();
        
        if (IsPasswordVerifyFailed(user, request.Password))
            return Unauthorized(new { Message = "Incorrect password!" });

        var authToken = authTokenGenerator.Generate();
        var session = await sessionsRepository.CreateOrUpdateSession(connection, user.Id, authToken);
        
        return Ok(session.ToAuthV1LoginResponse());
    }

    private bool IsPasswordVerifyFailed(User user, string password)
    {
        var userHashView = new UserHashView { Name = user.Name, Email = user.Email };
        return hasher.VerifyHashedPassword(userHashView, user.HashedPassword, password) ==
               PasswordVerificationResult.Failed;
    }
}