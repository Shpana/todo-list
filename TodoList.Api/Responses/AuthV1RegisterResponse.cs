namespace TodoList.Api;

public class AuthV1RegisterResponse
{
    public required int UserLoginSessionId { get; init; }
    public required string AuthToken { get; init; }
}

public static class AuthV1RegisterResponseMappings
{
    public static AuthV1RegisterResponse ToAuthV1RegisterResponse(
        this UserLoginSession session)
    {
        return new AuthV1RegisterResponse
        {
            UserLoginSessionId = session.UserId,
            AuthToken = session.AuthToken
        };
    }
}