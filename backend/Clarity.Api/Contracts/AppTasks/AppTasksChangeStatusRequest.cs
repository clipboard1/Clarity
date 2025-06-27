namespace Clarity.Api.Contracts.AppTasks;

public record AppTasksChangeStatusRequest(
    int NewStatus,
    Guid AppTaskId);