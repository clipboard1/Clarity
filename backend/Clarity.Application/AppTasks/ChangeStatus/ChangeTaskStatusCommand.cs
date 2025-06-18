using Clarity.Application.Abstractions;
using Clarity.Core.Enums;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.AppTasks.ChangeStatus;

public record ChangeTaskStatusCommand(
    Guid AppTaskId,
    int NewStatus)
    : ICommand;
    
public class ChangeTaskStatusCommandHandler : ICommandHandler<ChangeTaskStatusCommand>
{
    private readonly IAppTasksRepository _repository;

    public ChangeTaskStatusCommandHandler(IAppTasksRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(ChangeTaskStatusCommand command, CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(typeof(AppTaskStatus), command.NewStatus))
            return Result.Failure(Result.ToDict("Status", "Wrong status value"));

        var updateResult = await _repository.ChangeStatus(
            command.AppTaskId, 
            (AppTaskStatus)command.NewStatus,
            cancellationToken);
        if (!updateResult.IsSuccess)
            return Result.Failure(updateResult.Errors);

        return Result.Success();
    }
}