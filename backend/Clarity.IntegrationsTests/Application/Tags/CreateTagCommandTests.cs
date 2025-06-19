using Clarity.Application.Tags.Create;

namespace Clarity.IntegrationsTests.Application.Tags;

public class CreateTagCommandTests : BaseIntegrationTest
{
    private const string ValidTagName = "Important";
    
    public CreateTagCommandTests(IntegrationTestWebAppFactory factory) : base(factory){}

    [Fact]
    public async Task CreateTag_WithValidData_CreatesTag()
    {
        var task = await DataSeeder.AddTask();

        var command = new CreateTagCommand(
            ValidTagName,
            task.Id,
            task.UserId);

        var result = await CommandDispatcher.DispatchAsync<CreateTagCommand, int>(command);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value > 0);
    }

    [Fact]
    public async Task CreateTag_WithEmptyName_ReturnsValidationError()
    {
        var task = await DataSeeder.AddTask();

        var command = new CreateTagCommand(
            "",
            task.Id,
            task.UserId);

        var result = await CommandDispatcher.DispatchAsync<CreateTagCommand, int>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Name");
    }

    [Fact]
    public async Task CreateTag_WithWrongUserId_ReturnsAccessDenied()
    {
        var task = await DataSeeder.AddTask();

        var command = new CreateTagCommand(
            ValidTagName,
            task.Id,
            Guid.NewGuid());

        var result = await CommandDispatcher.DispatchAsync<CreateTagCommand, int>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "User" && e.Value.Contains("Access denied"));
    }
}