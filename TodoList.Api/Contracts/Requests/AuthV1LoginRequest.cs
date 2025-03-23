using System.Runtime.Serialization;

namespace TodoList.Api;

[DataContract]
public class AuthV1LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}