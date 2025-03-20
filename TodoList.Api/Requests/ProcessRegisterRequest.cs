using System.Text.Json.Serialization;

namespace TodoList.Api.Requests;

public class ProcessRegisterRequest
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}