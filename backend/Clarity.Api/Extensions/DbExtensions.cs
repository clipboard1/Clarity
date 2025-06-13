using Clarity.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Clarity.Api.Extensions;

public static class DbExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ClarityDbContext>();
        context.Database.Migrate();
    }
}