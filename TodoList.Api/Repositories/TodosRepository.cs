using System.Data;
using Dapper;

namespace TodoList.Api;

public interface ITodosRepository
{
    public Task<Todo> CreateTodo(
        IDbConnection connection, int userId, string title, string? description);
    public Task<Todo?> UpdateTodoWithId(IDbConnection connection, int id, string title, string? description);
    public Task DeleteTodoWithId(IDbConnection connection, int id);
    public Task<Todo?> GetTodoById(IDbConnection connection, int id);
    public Task<IEnumerable<Todo>> GetTodosPaginatedForUser(IDbConnection connection, int userId, int page, int limit);
}

public class TodosRepository : ITodosRepository
{
    public async Task<Todo> CreateTodo(
        IDbConnection connection, int userId, string title, string? description)
    {
        return await connection.QuerySingleAsync<Todo>(
            """
            INSERT INTO todos(user_id, title, description, created_at, updated_at) 
            VALUES (@UserId, @Title, @Description, @CreatedAt, @UpdatedAt)
            RETURNING *;
            """, new
            {
                UserId = userId,
                Title = title,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task<Todo?> UpdateTodoWithId(
        IDbConnection connection, int id, string title, string? description)
    {
        return await connection.QuerySingleOrDefaultAsync<Todo>(
            """
            UPDATE todos 
            SET title = @Title, description = @Description, updated_at = @UpdatedAt
            WHERE id = @Id
            RETURNING *;
            """, new
            {
                Id = id, 
                Title = title, 
                Description = description,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task DeleteTodoWithId(IDbConnection connection, int id)
    {
        await connection.ExecuteAsync(
            """
            DELETE FROM todos WHERE id = @Id;
            """, new { Id = id });
    }

    public async Task<Todo?> GetTodoById(IDbConnection connection, int id)
    {
        return await connection.QuerySingleOrDefaultAsync<Todo>(
            """
            SELECT * FROM todos WHERE id = @Id;
            """, new { Id = id });
    }

    public async Task<IEnumerable<Todo>> GetTodosPaginatedForUser(IDbConnection connection, int userId, int page, int limit)
    {
        return (await connection.QueryAsync<Todo>(
            """
            SELECT * FROM todos 
            WHERE user_id = @UserId
            ORDER BY created_at
            LIMIT @Limit OFFSET @Offset;
            """, new { UserId = userId, Limit = limit, Offset = (page - 1) * limit }));
    }
}