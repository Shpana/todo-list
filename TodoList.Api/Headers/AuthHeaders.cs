using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

public class AuthHeaders
{
    [FromHeader]
    [JsonPropertyName("XUserLoginSessionId")]
    public required int UserId { get; init; }
    [FromHeader]
    [JsonPropertyName("XAuthToken")] 
    public required string AuthToken { get; init; }
}