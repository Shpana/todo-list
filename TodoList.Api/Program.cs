using Carter;
using TodoList.Api.Database;
using TodoList.Api.Repositories;
using TodoList.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

builder.Services.AddSingleton<AuthorizationTokenGeneratorService>(_ => 
    new AuthorizationTokenGeneratorService(64));

builder.Services.AddSingleton<IDbConnectionFactory>(_ => 
    new NpgsqlDbConnectionFactory(builder.Configuration.GetConnectionString("todo-list-db")!));
builder.Services.AddSingleton<UsersRepository>();
builder.Services.AddSingleton<UserSessionsRepository>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapCarter();
app.Run();