using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
using TodoList.Api.Database;
using TodoList.Api.Models;

namespace TodoList.Api.Repositories;

public interface IUsersRepository
{
    public Task<User> CreateUser(
        IDbConnection connection, string name, string email, string hashedPassword);

    public Task<User?> GetUserById(IDbConnection connection, int id);
    public Task<User?> GetUserByEmail(IDbConnection connection, string email);
    
    public Task<bool> HasUserWithId(IDbConnection connection, int id);
    public Task<bool> HasUserWithEmail(IDbConnection connection, string email);
}

public class UsersRepository(
    ILogger<IUsersRepository> logger) : IUsersRepository
{
    public async Task<User> CreateUser(
        IDbConnection connection, string name, string email, string hashedPassword)
    {
        return await connection.QueryFirstAsync<User>(
            """
            INSERT INTO users (name, email, hashed_password, created_at, updated_at) 
            VALUES (@Name, @Email, @HashedPassword, @CreatedAt, @UpdatedAt)
            RETURNING *;
            """, new
            {
                Name = name,
                Email = email,
                HashedPassword = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task<User?> GetUserById(IDbConnection connection, int id)
    {
        return await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT * FROM users WHERE id=@Id LIMIT 1;
            """, new { Id = id });
    }
    
    public async Task<User?> GetUserByEmail(IDbConnection connection, string email)
    {
        return await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT * FROM users WHERE email=@Email LIMIT 1;
            """, new { Email = email });
    }

    public async Task<bool> HasUserWithId(IDbConnection connection, int id)
    {
        return (await GetUserById(connection, id)) is not null;
    }

    public async Task<bool> HasUserWithEmail(IDbConnection connection, string email)
    {
        return (await GetUserByEmail(connection, email)) is not null;
    }
}