using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

public class AuthHeaders
{
    [FromHeader(Name = "XUserLoginSessionId")]
    public required int UserId { get; init; }
    [FromHeader(Name = "XAuthToken")]
    public required string AuthToken { get; init; }
}