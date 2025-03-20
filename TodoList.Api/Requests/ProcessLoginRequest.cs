namespace TodoList.Api.Requests;

public class ProcessLoginRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
}