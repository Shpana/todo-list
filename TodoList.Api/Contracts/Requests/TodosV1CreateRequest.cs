using System.Runtime.Serialization;

namespace TodoList.Api;

[DataContract]
public class TodosV1CreateRequest
{
    public required string Title { get; init; }
    public string? Description { get; init; }
}