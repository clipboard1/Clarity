using Clarity.Application.AppTasks.Delete;

namespace Clarity.IntegrationsTests.Application.AppTasks;

public class DeleteTaskCommandTests : BaseIntegrationTest
{
    
    public DeleteTaskCommandTests(IntegrationTestWebAppFactory factory) : base(factory){}

    [Fact]
    public async Task DeleteTask_WithValidData_DeletesTask()
    {
        var task = await DataSeeder.AddTask();

        var command = new DeleteTaskCommand(task.Id, task.UserId);

        var result = await CommandDispatcher.DispatchAsync<DeleteTaskCommand>(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteTask_WithWrongUserId_ReturnsAccessDenied()
    {
        var task = await DataSeeder.AddTask();

        var command = new DeleteTaskCommand(task.Id, Guid.NewGuid()); // Другой пользователь

        var result = await CommandDispatcher.DispatchAsync<DeleteTaskCommand>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "User");
    }

    [Fact]
    public async Task DeleteTask_WithEmptyTaskId_ReturnsValidationError()
    {
        var command = new DeleteTaskCommand(Guid.Empty, Guid.NewGuid());

        var result = await CommandDispatcher.DispatchAsync<DeleteTaskCommand>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Id");
    }

    [Fact]
    public async Task DeleteTask_TaskNotFound_ReturnsError()
    {
        var user = await DataSeeder.AddUser();

        var command = new DeleteTaskCommand(Guid.NewGuid(), user.Id);

        var result = await CommandDispatcher.DispatchAsync<DeleteTaskCommand>(command);

        Assert.False(result.IsSuccess);
    }

}