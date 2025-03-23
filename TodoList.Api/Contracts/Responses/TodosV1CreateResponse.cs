using System.Runtime.Serialization;

namespace TodoList.Api;

[DataContract]
public class TodosV1CreateResponse
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}

public static class TodosV1CreateResponseMappings
{
    public static TodosV1CreateResponse ToTodosV1CreateResponse(this Todo todo)
    {
        return new TodosV1CreateResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description
        };
    }
}