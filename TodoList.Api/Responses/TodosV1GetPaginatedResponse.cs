namespace TodoList.Api;

public class TodosV1GetPaginatedTodoView
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}

public class TodosV1GetPaginatedResponse
{
    public required IEnumerable<TodosV1GetPaginatedTodoView> Data { get; init; }
    public required int Page { get; init; }
    public required int Limit { get; init; }
    public required int Total { get; init; }
}

public static class TodosV1GetPaginatedResponseMappings
{
    public static TodosV1GetPaginatedTodoView ToTodosV1GetPaginatedTodoView(
        this Todo todo)
    {
        return new TodosV1GetPaginatedTodoView
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description
        };
    }
    
    public static TodosV1GetPaginatedResponse ToTodosV1PaginatedResponse(
        this IEnumerable<Todo> todos, int page, int limit)
    {
        return new TodosV1GetPaginatedResponse
        {
            Data = todos.Select(todo => todo.ToTodosV1GetPaginatedTodoView()),
            Page = page,
            Limit = limit,
            Total = todos.Count()
        };
    }
}