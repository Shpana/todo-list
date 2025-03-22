namespace TodoList.Api.Requests;

public class ProcessLoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}