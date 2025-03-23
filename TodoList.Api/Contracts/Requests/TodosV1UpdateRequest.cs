using System.Runtime.Serialization;

namespace TodoList.Api;

[DataContract]
public class TodosV1UpdateRequest
{
    public required string Title { get; init; }
    public string? Description { get; init; }
}