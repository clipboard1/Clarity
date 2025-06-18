using Clarity.Core.Models;

namespace Clarity.UnitTests.Core;

public class TagTests
{
    private static readonly Guid ValidTaskId = Guid.NewGuid();
    private const string ValidName = "Work";

    [Fact]
    public void Tag_ShouldCreate_WhenAllFieldsValid()
    {
        var result = Tag.Create(1, ValidName, ValidTaskId);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void Tag_ShouldFail_WhenAppTaskIdIsEmpty()
    {
        var result = Tag.Create(1, ValidName, Guid.Empty);
        
        Assert.False(result.IsSuccess);
        Assert.Contains("Id", result.Errors.Keys);
    }

    [Fact]
    public void Tag_ShouldFail_WhenNameIsEmpty()
    {
        var result = Tag.Create(1, "", ValidTaskId);
        
        Assert.False(result.IsSuccess);
        Assert.Contains("Name", result.Errors.Keys);
    }

    [Fact]
    public void Tag_ShouldFail_WhenNameIsTooLong()
    {
        var longName = new string('x', Tag.MAX_NAME_LENGTH + 1);

        var result = Tag.Create(1, longName, ValidTaskId);

        Assert.False(result.IsSuccess);
        Assert.Contains("Name", result.Errors.Keys);
    }

    [Fact]
    public void Tag_ShouldFail_WhenNameIsNull()
    {
        var result = Tag.Create(1, null!, ValidTaskId);

        Assert.False(result.IsSuccess);
        Assert.Contains("Name", result.Errors.Keys);
    }
}
