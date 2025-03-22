namespace TodoList.Api;

public class TodosV1UpdateRequest
{
    public required string Title { get; init; }
    public string? Description { get; init; }
}