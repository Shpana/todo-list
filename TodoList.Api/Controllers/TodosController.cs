using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace TodoList.Api;

[Route("todos/v1")]
[ApiController]
public class TodosController(
    IDbConnectionFactory factory,
    IUserLoginSessionsRepository sessionsRepository,
    ITodosRepository todosRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] AuthHeaders headers, 
        [FromBody] TodosV1CreateRequest request)
    {
        using var connection = await factory.CreateConnectionAsync();

        var sessionResult = await ValidateLoginSession(connection, headers);
        if (sessionResult is not OkResult)
            return sessionResult; 

        var todo = await todosRepository.CreateTodo(
            connection, headers.UserId, request.Title, request.Description);
        return Ok(todo.ToTodosV1CreateResponse());
    }

    [HttpPut]
    [Route("{todoId}")]
    public async Task<IActionResult> Update(
        [FromQuery] AuthHeaders headers,
        int todoId,
        [FromBody] TodosV1UpdateRequest request)
    {
        using var connection = await factory.CreateConnectionAsync();

        var sessionResult = await ValidateLoginSession(connection, headers);
        if (sessionResult is not OkResult)
            return sessionResult;
        var permissionsResult = await UserHasEnoughPermissions(connection, headers.UserId, todoId);
        if (permissionsResult is not OkResult)
            return permissionsResult;
        
        var todo = await todosRepository.UpdateTodoWithId(
            connection, todoId, request.Title, request.Description);
        if (todo == null)
            return NotFound();
        return Ok(todo.ToTodosV1UpdateResponse());
    }

    [HttpDelete]
    [Route("{todoId}")]
    public async Task<IActionResult> Delete([FromQuery] AuthHeaders headers, int todoId)
    {
        using var connection = await factory.CreateConnectionAsync();

        var sessionResult = await ValidateLoginSession(connection, headers);
        if (sessionResult is not OkResult)
            return sessionResult;
        var permissionsResult = await UserHasEnoughPermissions(connection, headers.UserId, todoId);
        if (permissionsResult is not OkResult)
            return permissionsResult;
 
        await todosRepository.DeleteTodoWithId(connection, todoId);
        return NoContent();
    }
    
    [HttpGet]
    [Route("{todoId}")]
    public async Task<IActionResult> GetById(
        [FromQuery] AuthHeaders headers,
        int todoId)
    {
        using var connection = await factory.CreateConnectionAsync();

        var sessionResult = await ValidateLoginSession(connection, headers);
        if (sessionResult is not OkResult)
            return sessionResult;
        var permissionsResult = await UserHasEnoughPermissions(connection, headers.UserId, todoId);
        if (permissionsResult is not OkResult)
            return permissionsResult;
 
        var todo = await todosRepository.GetTodoById(connection, todoId);
        if (todo == null)
            return NotFound();
        return Ok(todo.ToTodosV1GetByIdResponse());
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginated(
        [FromQuery] AuthHeaders headers,
        [Required] [FromQuery] int page,
        [Required] [FromQuery] int limit)
    {
        using var connection = await factory.CreateConnectionAsync();

        var sessionResult = await ValidateLoginSession(connection, headers);
        if (sessionResult is not OkResult)
            return sessionResult;
 
        var todos = await todosRepository.GetTodosPaginatedForUser(connection, headers.UserId, page, limit);
        return Ok(todos.ToTodosV1PaginatedResponse(page, limit));
    }

    private async Task<IActionResult> ValidateLoginSession(
        IDbConnection connection, AuthHeaders headers)
    {
        var session = await sessionsRepository.GetSessionByUserId(connection, headers.UserId);
        if (session == null)
            return NotFound(new { Message = "Session not found" });
        if (session.AuthToken != headers.AuthToken)
            return Unauthorized();
        return Ok();
    }

    private async Task<IActionResult> UserHasEnoughPermissions(IDbConnection connection, int userId, int todoId)
    {
        var todo = await todosRepository.GetTodoById(connection, todoId);
        if (todo == null)
            return NotFound(new { Message = "Todo not found" });
        return todo.UserId == userId ? Ok() : Unauthorized();
    }
}