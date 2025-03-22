using Microsoft.AspNetCore.Identity;
using TodoList.Api.Database;
using TodoList.Api.Repositories;
using TodoList.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddTransient<PasswordHasher<UserLoginInfo>>();
builder.Services.AddTransient<AuthTokenGeneratorService>(_ => 
    new AuthTokenGeneratorService(int.Parse(builder.Configuration["AuthTokenLength"]!)));

builder.Services.AddSingleton<IDbConnectionFactory>(_ => 
    new NpgsqlDbConnectionFactory(builder.Configuration.GetConnectionString("todo-list-db")!));
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IUserSessionsRepository, UserSessionsRepository>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();