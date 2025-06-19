using Clarity.Application.Tags.Delete;

namespace Clarity.IntegrationsTests.Application.Tags;

public class DeleteTagCommandTest : BaseIntegrationTest
{
    public DeleteTagCommandTest(IntegrationTestWebAppFactory factory) : base(factory){}
    
    [Fact]
    public async Task DeleteTag_WithValidId_Succeeds()
    {
        var tag = await DataSeeder.AddTag();

        var command = new DeleteTagCommand(tag.Id);

        var result = await CommandDispatcher.DispatchAsync(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteTag_WithInvalidId_ReturnsValidationError()
    {
        var command = new DeleteTagCommand(0);

        var result = await CommandDispatcher.DispatchAsync(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Id");
    }

}