using NpgsqlTypes;

namespace TodoList.Api.DTOs;

public class User
{
    [PgName("id")]
    public required int Id { get; init; }
    [PgName("name")]
    public required string Name { get; set; }
    [PgName("email")]
    public required string Email { get; set; }
    [PgName("password")]
    public required string Password { get; set; }
}