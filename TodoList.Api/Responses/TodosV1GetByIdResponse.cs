namespace TodoList.Api;

public class TodosV1GetByIdResponse
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}

public static class TodosV1GetByIdResponseMappings
{
    public static TodosV1GetByIdResponse ToTodosV1GetByIdResponse(this Todo todo)
    {
        return new TodosV1GetByIdResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description
        };
    }
}