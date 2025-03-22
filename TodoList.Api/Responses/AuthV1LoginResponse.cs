namespace TodoList.Api;

public class AuthV1LoginResponse
{
    public required int UserLoginSessionId { get; init; }
    public required string AuthToken { get; init; } 
}

public static class AuthV1LoginResponseMappings
{
    public static AuthV1LoginResponse ToAuthV1LoginResponse(
        this UserLoginSession session)
    {
        return new AuthV1LoginResponse
        {
            UserLoginSessionId = session.UserId,
            AuthToken = session.AuthToken
        };
    }
}