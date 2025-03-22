using System.Data;
using Dapper;

namespace TodoList.Api;

public interface IUserLoginSessionsRepository
{
    public Task<UserLoginSession> CreateOrUpdateSession(
        IDbConnection connection, int userId, string authToken);
    public Task<UserLoginSession?> GetSessionByUserId(IDbConnection connection, int userId);
}

public class UserLoginLoginSessionsRepository : IUserLoginSessionsRepository
{
    public async Task<UserLoginSession> CreateOrUpdateSession(
        IDbConnection connection, int userId, string authToken)
    {
        return await connection.QuerySingleAsync<UserLoginSession>(
            """
            INSERT INTO user_login_sessions (user_id, auth_token, created_at, updated_at)
            VALUES (@UserId, @Token, @CreatedAt, @UpdatedAt)
            ON CONFLICT (user_id) DO UPDATE SET auth_token = @Token, updated_at = @UpdatedAt
            RETURNING *;
            """, new
            {
                UserId = userId,
                Token = authToken,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task<UserLoginSession?> GetSessionByUserId(
        IDbConnection connection, int userId)
    {
        return await connection.QuerySingleOrDefaultAsync<UserLoginSession>(
            """
            SELECT * FROM user_login_sessions WHERE user_id = @UserId;
            """, new { UserId = userId });
    }
}