namespace Clarity.Api.Contracts.Tags;

public record TagResponse(
    int Id,
    string Name,
    Guid AppTaskId);