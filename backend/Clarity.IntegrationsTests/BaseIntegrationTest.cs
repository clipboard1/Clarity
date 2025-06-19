using Clarity.Application.Abstractions;
using Clarity.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Clarity.IntegrationsTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly ClarityDbContext DbContext;
    protected readonly ICommandDispatcher CommandDispatcher;
    protected readonly IQueryDispatcher QueryDispatcher;
    protected readonly DataSeeder DataSeeder;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        CommandDispatcher = _scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        QueryDispatcher = _scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
        DbContext = _scope.ServiceProvider.GetRequiredService<ClarityDbContext>();
        DataSeeder = new DataSeeder(DbContext);
    }
}