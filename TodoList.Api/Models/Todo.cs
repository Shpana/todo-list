namespace TodoList.Api;

public class Todo
{
    public required int Id { get; init; }
    public required int UserId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}