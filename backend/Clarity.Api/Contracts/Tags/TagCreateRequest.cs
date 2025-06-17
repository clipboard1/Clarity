namespace Clarity.Api.Contracts.Tags;

public record TagCreateRequest(
    string Name,
    Guid AppTaskId);