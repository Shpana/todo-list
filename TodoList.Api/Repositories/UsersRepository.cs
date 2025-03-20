using System.Data;
using Dapper;
using TodoList.Api.Database;
using TodoList.Api.DTOs;

namespace TodoList.Api.Repositories;

public class UsersRepository
{
    private readonly ILogger<UsersRepository> _logger;

    public UsersRepository(ILogger<UsersRepository> logger)
    {
        _logger = logger;
    }

    public async Task<long> CreateUser(
        IDbConnection connection, string name, string email, string hashedPassword)
    {
         return await connection.ExecuteAsync(
            """
            INSERT INTO users (name, email, password, created_at, updated_at) 
            VALUES (@Name, @Email, @Password, @CreatedAt, @UpdatedAt)
            RETURNING (id, name, email);
            """, new
            {
                Name = name,
                Email = email,
                Password = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
    }
    
    public async Task<User?> GetUserByEmail(IDbConnection connection, string email)
    {
        return await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT * FROM users WHERE email=@Email LIMIT 1;
            """, new { Email = email });
    }
}