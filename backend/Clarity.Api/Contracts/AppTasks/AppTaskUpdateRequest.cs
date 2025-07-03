using Clarity.Core.Enums;

namespace Clarity.Api.Contracts.AppTasks;

public record AppTaskUpdateRequest(
    Guid Id,
    string? Title,
    string? Description,
    AppTaskStatus? Status);