using Carter;
using TodoList.Api.Database;
using TodoList.Api.Repositories;
using TodoList.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton<AuthTokenGeneratorService>(_ => 
    new AuthTokenGeneratorService(64));

builder.Services.AddSingleton<IDbConnectionFactory>(_ => 
    new NpgsqlDbConnectionFactory(builder.Configuration.GetConnectionString("todo-list-db")!));
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IUserSessionsRepository, UserSessionsRepository>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();