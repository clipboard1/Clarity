using Clarity.Api.Extensions;
using Clarity.Persistence;
using Microsoft.EntityFrameworkCore;
using Clarity.Infrastructure.Authentication;
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
builder.Services.AddInfrastructure();
builder.Services.AddRepositories();
builder.Services.AddDispatchers();
builder.Services.AddAuthCommands();
builder.Services.AddAppTaskCommands();
builder.Services.AddAppTaskQueries();

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
app.UseSecureJwt();
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
