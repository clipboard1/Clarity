using Clarity.Persistence;
using Microsoft.EntityFrameworkCore;
using Clarity.Core.Abstractions;
using Clarity.Infrastructure.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Configuration.AddEnvironmentVariables();

string? connectionString = builder.Configuration.GetConnectionString(nameof(ClarityDbContext));

builder.Services.AddDbContext<ClarityDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClarityDbContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
