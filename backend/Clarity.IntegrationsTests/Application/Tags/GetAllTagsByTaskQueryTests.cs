using Clarity.Application.Tags.GetAllByTask;
using Clarity.Core.Models;

namespace Clarity.IntegrationsTests.Application.Tags;

public class GetAllTagsByTaskQueryTests : BaseIntegrationTest
{
    public GetAllTagsByTaskQueryTests(IntegrationTestWebAppFactory factory) : base(factory){}

    [Fact]
    public async Task GetAllTagsByTask_WithValidData_ReturnsTags()
    {
        var task = await DataSeeder.AddTask();
        var tag1 = await DataSeeder.AddTag(task.Id);
        var tag2 = await DataSeeder.AddTag(task.Id);

        var query = new GetAllTagsByTaskQuery(task.Id, task.UserId);

        var result = await QueryDispatcher.DispatchAsync<GetAllTagsByTaskQuery, List<Tag>>(query);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value);
        Assert.Contains(result.Value, t => t.Id == tag1.Id);
        Assert.Contains(result.Value, t => t.Id == tag2.Id);
    }

    [Fact]
    public async Task GetAllTagsByTask_WithWrongUserId_ReturnsAccessDenied()
    {
        var task = await DataSeeder.AddTask();

        var query = new GetAllTagsByTaskQuery(task.Id, Guid.NewGuid());

        var result = await QueryDispatcher.DispatchAsync<GetAllTagsByTaskQuery, List<Tag>>(query);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "User");
    }

}