using Clarity.Core.Enums;
using Clarity.Core.Models;

namespace Clarity.UnitTests.Core;

public class AppTaskTests
{
    private static readonly Guid ValidId = Guid.NewGuid();
    private static readonly Guid ValidUserId = Guid.NewGuid();
    private const string ValidTitle = "Test task";
    private const string ValidDescription = "Task description";
    private static readonly List<Tag> EmptyTags = [];

    [Fact]
    public void AppTask_ShouldCreate_WhenAllFieldsValid()
    {
        var result = AppTask.Create(
            ValidId,
            ValidUserId,
            ValidTitle,
            ValidDescription,
            EmptyTags);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void AppTask_ShouldFail_WhenIdIsEmpty()
    {
        var result = AppTask.Create(
            Guid.Empty,
            ValidUserId,
            ValidTitle,
            ValidDescription,
            EmptyTags);

        Assert.False(result.IsSuccess);
        Assert.Contains("Id", result.Errors.Keys);
    }

    [Fact]
    public void AppTask_ShouldFail_WhenUserIdIsEmpty()
    {
        var result = AppTask.Create(
            ValidId,
            Guid.Empty,
            ValidTitle,
            ValidDescription,
            EmptyTags);

        Assert.False(result.IsSuccess);
        Assert.Contains("Id", result.Errors.Keys);
    }

    [Fact]
    public void AppTask_ShouldFail_WhenTitleIsEmpty()
    {
        var result = AppTask.Create(
            ValidId,
            ValidUserId,
            "",
            ValidDescription,
            EmptyTags);

        Assert.False(result.IsSuccess);
        Assert.Contains("Title", result.Errors.Keys);
    }

    [Fact]
    public void AppTask_ShouldFail_WhenTitleIsTooLong()
    {
        var longTitle = new string('x', AppTask.MAX_TITLE_LENGTH + 1);

        var result = AppTask.Create(
            ValidId,
            ValidUserId,
            longTitle,
            ValidDescription,
            EmptyTags);

        Assert.False(result.IsSuccess);
        Assert.Contains("Title", result.Errors.Keys);
    }

    [Fact]
    public void AppTask_ShouldFail_WhenDescriptionIsTooLong()
    {
        var longDescription = new string('x', AppTask.MAX_DESCRIPTION_LENGTH + 1);

        var result = AppTask.Create(
            ValidId,
            ValidUserId,
            ValidTitle,
            longDescription,
            EmptyTags);

        Assert.False(result.IsSuccess);
        Assert.Contains("Description", result.Errors.Keys);
    }

    [Fact]
    public void AppTask_ShouldUseEmptyDescription_WhenNullPassed()
    {
        var result = AppTask.Create(
            ValidId,
            ValidUserId,
            ValidTitle,
            null!,
            EmptyTags);

        Assert.True(result.IsSuccess);
        Assert.Equal(string.Empty, result.Value.Description);
    }
}
