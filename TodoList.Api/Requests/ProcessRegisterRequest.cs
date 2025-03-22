namespace TodoList.Api.Requests;

public class ProcessRegisterRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}