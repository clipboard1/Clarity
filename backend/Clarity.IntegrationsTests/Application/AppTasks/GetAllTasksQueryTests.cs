using Clarity.Application.AppTasks.GetAll;
using Clarity.Core.Models;

namespace Clarity.IntegrationsTests.Application.AppTasks;

public class GetAllTasksQueryTests : BaseIntegrationTest
{
    public GetAllTasksQueryTests(IntegrationTestWebAppFactory factory) : base(factory){}

    [Fact]
    public async Task GetAllTasks_WithValidUserId_ReturnsTaskList()
    {
        var user = await DataSeeder.AddUser();

        var query = new GetAllTasksQuery(user.Id);

        var result = await QueryDispatcher.DispatchAsync<GetAllTasksQuery, List<AppTask>>(query);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.IsType<List<AppTask>>(result.Value);
    }

    [Fact]
    public async Task GetAllTasks_WithEmptyUserId_ReturnsValidationError()
    {
        var query = new GetAllTasksQuery(Guid.Empty);

        var result = await QueryDispatcher.DispatchAsync<GetAllTasksQuery, List<AppTask>>(query);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "UserId");
    }

}