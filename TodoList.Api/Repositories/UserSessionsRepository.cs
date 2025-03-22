using System.Data;
using Dapper;
using TodoList.Api.Models;

namespace TodoList.Api.Repositories;

public interface IUserSessionsRepository
{
    public Task<UserLoginSession> CreateOrUpdateSession(IDbConnection connection, int userId, string authToken);
}

public class UserSessionsRepository : IUserSessionsRepository
{
    public async Task<UserLoginSession> CreateOrUpdateSession(
        IDbConnection connection, int userId, string authToken)
    {
        return await connection.QueryFirstAsync<UserLoginSession>(
            """
            INSERT INTO user_login_sessions (id, auth_token, created_at, updated_at)
            VALUES (@Id, @Token, @CreatedAt, @UpdatedAt)
            ON CONFLICT (id) DO UPDATE SET auth_token = @Token, updated_at = @UpdatedAt
            RETURNING *;
            """, new
            {
                Id = userId,
                Token = authToken,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
    }
}