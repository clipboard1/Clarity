using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.AppTasks.Delete;

public record DeleteTaskCommand(Guid Id)
    : ICommand;
    
public class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand>
{
    private readonly IAppTasksRepository _repository;

    public DeleteTaskCommandHandler(IAppTasksRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(DeleteTaskCommand taskCommand,
        CancellationToken cancellationToken = default)
    {
        var getResult = await _repository.GetById(taskCommand.Id,
            cancellationToken);
        if (!getResult.IsSuccess)
            return Result.Failure(getResult.Errors);

        var deleteResult = await _repository.Delete(taskCommand.Id,
            cancellationToken);
        if (!deleteResult.IsSuccess)
            return Result.Failure(deleteResult.Errors);
        
        return Result.Success();
    }
}