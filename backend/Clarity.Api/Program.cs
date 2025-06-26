using Clarity.Api.Extensions;
using Clarity.Persistence;
using Microsoft.EntityFrameworkCore;
using Clarity.Infrastructure.Authentication;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

string? connectionString = builder.Configuration.GetConnectionString(nameof(ClarityDbContext));

builder.Services.AddDbContext<ClarityDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

const string corsPolicyName = "_myCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName, builder =>
        builder
            .WithOrigins("https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddInfrastructure();
builder.Services.AddRepositories();
builder.Services.AddDispatchers();
builder.Services.AddAuthCommands();
builder.Services.AddAppTaskCommandsAndQueries();
builder.Services.AddTagCommandsAndQueries();
builder.Services.AddAutoMappers();
builder.Services.AddApiAuthentication();
builder.Services.AddApiValidators();
builder.Services.AddAppValidators();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString!);

var app = builder.Build();

app.MigrateDb();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseHttpsRedirection();
app.UseSecureJwt();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(corsPolicyName);

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public partial class Program {} 