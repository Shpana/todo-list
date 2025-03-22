namespace TodoList.Api;

public class User
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string HashedPassword { get; set; }
}