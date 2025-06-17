using Clarity.Api.Contracts.Tags;
using Clarity.Core.Enums;

namespace Clarity.Api.Contracts.AppTasks;

public record AppTaskResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime CreationDate,
    DateTime Deadline,
    ICollection<TagResponse> Tags,
    AppTaskStatus Status);