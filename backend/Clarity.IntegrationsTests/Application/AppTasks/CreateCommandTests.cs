using Clarity.Application.AppTasks.Create;
using Clarity.Core.Enums;
using Clarity.Core.Models;

namespace Clarity.IntegrationsTests.Application.AppTasks;

public class CreateTaskCommandTests : BaseIntegrationTest
{
    private const string ValidTitle = "Valid Task Title";
    private const string ValidDescription = "This is a valid task description";

    public CreateTaskCommandTests(IntegrationTestWebAppFactory factory) : base(factory) {}

    [Fact]
    public async Task CreateTask_WithValidData_CreatesTask()
    {
        var user = await DataSeeder.AddUser();
        var command = new CreateTaskCommand(
            user.Id,
            ValidTitle,
            ValidDescription,
            [],
            AppTaskStatus.NotStarted);

        var result = await CommandDispatcher.DispatchAsync<CreateTaskCommand, Guid>(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CreateTask_WithTooLongTitle_ReturnsValidationError()
    {
        var user = await DataSeeder.AddUser();
        var command = new CreateTaskCommand(
            user.Id,
            new string('*', AppTask.MAX_TITLE_LENGTH + 1),
            ValidDescription,
            [],
            AppTaskStatus.NotStarted);

        var result = await CommandDispatcher.DispatchAsync<CreateTaskCommand, Guid>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Title");
    }
    
    [Fact]
    public async Task CreateTask_WithEmptyUserId_ReturnsValidationError()
    {
        var command = new CreateTaskCommand(
            Guid.Empty,
            ValidTitle,
            ValidDescription,
            [],
            AppTaskStatus.NotStarted);

        var result = await CommandDispatcher.DispatchAsync<CreateTaskCommand, Guid>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "UserId");
    }
}
