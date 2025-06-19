using Clarity.Application.AppTasks.GetById;
using Clarity.Core.Models;

namespace Clarity.IntegrationsTests.Application.AppTasks;

public class GetByIdQueryTests : BaseIntegrationTest
{
    public GetByIdQueryTests(IntegrationTestWebAppFactory factory) : base(factory){}

    [Fact]
    public async Task GetTaskById_WithValidData_ReturnsTask()
    {
        var task = await DataSeeder.AddTask();

        var query = new GetTaskByIdQuery(task.Id, task.UserId);

        var result = await QueryDispatcher.DispatchAsync<GetTaskByIdQuery, AppTask>(query);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(task.Id, result.Value.Id);
        Assert.Equal(task.UserId, result.Value.UserId);
    }

    [Fact]
    public async Task GetTaskById_WithEmptyTaskId_ReturnsValidationError()
    {
        var query = new GetTaskByIdQuery(Guid.Empty, Guid.NewGuid());

        var result = await QueryDispatcher.DispatchAsync<GetTaskByIdQuery, AppTask>(query);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Id");
    }

    [Fact]
    public async Task GetTaskById_WithWrongUserId_ReturnsAccessDenied()
    {
        var task = await DataSeeder.AddTask();

        var query = new GetTaskByIdQuery(task.Id, Guid.NewGuid());

        var result = await QueryDispatcher.DispatchAsync<GetTaskByIdQuery, AppTask>(query);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "User" && e.Value.Contains("Access denied"));
    }

}