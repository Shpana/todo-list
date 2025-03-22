namespace TodoList.Api;

public class TodosV1UpdateResponse
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}

public static class TodosV1UpdateResponseMappings
{
    public static TodosV1UpdateResponse ToTodosV1UpdateResponse(this Todo todo)
    {
        return new TodosV1UpdateResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description
        };
    }
}