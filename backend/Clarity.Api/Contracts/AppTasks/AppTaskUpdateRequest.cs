using Clarity.Core.Enums;

namespace Clarity.Api.Contracts.AppTasks;

public record AppTaskUpdateRequest(
    string? Title,
    string? Description,
    DateTime? Deadline,
    AppTaskStatus? Status);