using Clarity.Application.AppTasks.ChangeStatus;
using Clarity.Core.Enums;

namespace Clarity.IntegrationsTests.Application.AppTasks;

public class ChangeTaskStatusCommandTests: BaseIntegrationTest
{
    private const int ValidStatus = (int)AppTaskStatus.Done;
    private const int InvalidStatus = 999;
    
    public ChangeTaskStatusCommandTests(IntegrationTestWebAppFactory factory) : base(factory) {}

    [Fact]
    public async Task ChangeTaskStatus_WithValidStatus_ChangesStatus()
    {
        var task = await DataSeeder.AddTask();
        var command = new ChangeTaskStatusCommand(task.Id, ValidStatus);

        var result = await CommandDispatcher.DispatchAsync(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ChangeTaskStatus_WithInvalidStatus_ReturnsError()
    {
        var task = await DataSeeder.AddTask();
        var command = new ChangeTaskStatusCommand(task.Id, InvalidStatus);

        var result = await CommandDispatcher.DispatchAsync(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Status");
    }

}