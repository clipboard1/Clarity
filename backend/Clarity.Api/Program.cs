using Clarity.Api.Extensions;
using Clarity.Application;
using Clarity.Application.Abstractions;
using Clarity.Application.LogoutAllDevices;
using Clarity.Application.Users.Login;
using Clarity.Application.Users.LoginByRefreshToken;
using Clarity.Application.Users.Logout;
using Clarity.Application.Users.Register;
using Clarity.Persistence;
using Microsoft.EntityFrameworkCore;
using Clarity.Core.Abstractions;
using Clarity.Core.Dto;
using Clarity.Infrastructure.Authentication;
using Clarity.Persistence.Abstractions;
using Clarity.Persistence.Repositories;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

string? connectionString = builder.Configuration.GetConnectionString(nameof(ClarityDbContext));

builder.Services.AddDbContext<ClarityDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddScoped<ICommandHandler<RegisterCommand>, RegisterCommandHandler>();
builder.Services.AddScoped<ICommandHandler<LoginCommand, AuthTokens>, LoginCommandHandler>();
builder.Services.AddScoped<ICommandHandler<LogoutCommand>, LogoutCommandHandler>();
builder.Services.AddScoped<ICommandHandler<LogoutAllDevicesCommand>, LogoutAllDevicesCommandHandler>();
builder.Services.AddScoped<ICommandHandler<LoginByRefreshTokenCommand, AuthTokens>,LoginByRefreshTokenCommandHandler>();
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();

builder.Services.AddApiAuthentication(
    builder.Services
        .BuildServiceProvider()
        .GetRequiredService<IOptions<JwtOptions>>());

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClarityDbContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(x => 
{
    x.WithHeaders().AllowAnyHeader();
    x.AllowAnyMethod();
    x.AllowCredentials();
});

app.MapControllers();

app.Run();
