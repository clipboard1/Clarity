using Clarity.Application.AppTasks.Update;
using Clarity.Core.Enums;
using Clarity.Core.Models;

namespace Clarity.IntegrationsTests.Application.AppTasks;

public class UpdateTaskCommandTests : BaseIntegrationTest
{
    private const string UpdatedTitle = "Updated title";
    private const string UpdatedDescription = "Updated description";
    
    public UpdateTaskCommandTests(IntegrationTestWebAppFactory factory) : base(factory){}
    
    [Fact]
    public async Task UpdateTask_WithValidData_UpdatesTask()
    {
        var task = await DataSeeder.AddTask();
        var command = new UpdateTaskCommand(
            task.Id,
            task.UserId,
            new AppTaskUpdate(
                UpdatedTitle,
                UpdatedDescription,
                AppTaskStatus.InProgress));

        var result = await CommandDispatcher.DispatchAsync<UpdateTaskCommand>(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateTask_WithInvalidUserId_ReturnsAccessDenied()
    {
        var task = await DataSeeder.AddTask();
        var command = new UpdateTaskCommand(
            task.Id,
            Guid.NewGuid(),
            new AppTaskUpdate(
                UpdatedTitle,
                null,
                null));

        var result = await CommandDispatcher.DispatchAsync<UpdateTaskCommand>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "User");
    }

    [Fact]
    public async Task UpdateTask_WithEmptyTaskId_ReturnsValidationError()
    {
        var user = await DataSeeder.AddUser();
        var command = new UpdateTaskCommand(
            Guid.Empty,
            user.Id,
            new AppTaskUpdate(
                UpdatedTitle,
                null,
                null));

        var result = await CommandDispatcher.DispatchAsync<UpdateTaskCommand>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Id");
    }
}