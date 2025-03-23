using System.Runtime.Serialization;

namespace TodoList.Api;

[DataContract]
public class AuthV1RegisterRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}