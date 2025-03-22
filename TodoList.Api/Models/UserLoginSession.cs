namespace TodoList.Api;

public class UserLoginSession
{
    public required int UserId { get; init; }
    public required string AuthToken { get; init; }
    public required DateTime UpdatedAt { get; init; }
}