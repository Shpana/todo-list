using System.Data;
using Dapper;

namespace TodoList.Api.Repositories;

public interface IUserSessionsRepository
{
    public Task<long> CreateSession(IDbConnection connection, int userId, string authToken);
    public Task<long> CreateOrUpdateSession(IDbConnection connection, int userId, string authToken);
}

public class UserSessionsRepository : IUserSessionsRepository
{
    public async Task<long> CreateSession(
        IDbConnection connection, int userId, string authToken)
    {
        return await connection.ExecuteAsync(
            """
            INSERT INTO user_sessions (id, token, created_at, updated_at)
            VALUES (@Id, @Token, @CreatedAt, @UpdatedAt);
            """, new
            {
                Id = userId,
                Token = authToken,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task<long> CreateOrUpdateSession(
        IDbConnection connection, int userId, string authToken)
    {
        return await connection.ExecuteAsync(
            """
            INSERT INTO user_sessions (id, token, created_at, updated_at)
            VALUES (@Id, @Token, @CreatedAt, @UpdatedAt)
            ON CONFLICT (id) DO UPDATE SET token = @Token, updated_at = @UpdatedAt;
            """, new
            {
                Id = userId,
                Token = authToken,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
    }
}