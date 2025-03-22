namespace TodoList.Api;

public class TodosV1CreateRequest
{
    public required string Title { get; init; }
    public string? Description { get; init; }
}