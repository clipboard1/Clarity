using Clarity.Core.Enums;
using Clarity.Core.Models;

namespace Clarity.Api.Contracts.AppTasks;

public record AppTaskResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime CreationDate,
    DateTime Deadline,
    ICollection<Tag> Tags,
    AppTaskStatus Status);