namespace TodoList.Api.Requests;

public class AuthV1RegisterRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}