using Clarity.Core.Enums;

namespace Clarity.Api.Contracts.AppTasks;

public record AppTaskCreateRequest(
    string Title,
    string Description,
    AppTaskStatus Status);