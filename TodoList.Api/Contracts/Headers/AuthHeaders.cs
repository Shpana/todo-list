using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace TodoList.Api;

[DataContract]
public class AuthHeaders
{
    [FromHeader(Name = "XUserLoginSessionId")]
    public required int UserId { get; init; }
    [FromHeader(Name = "XAuthToken")]
    public required string AuthToken { get; init; }
}