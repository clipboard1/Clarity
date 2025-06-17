using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.AppTasks.Delete;

public record DeleteTaskCommand(
    Guid Id,
    Guid UserId)
    : ICommand;
    
public class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand>
{
    private readonly IAppTasksRepository _repository;

    public DeleteTaskCommandHandler(IAppTasksRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(DeleteTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        var getResult = await _repository.GetById(command.Id,
            cancellationToken);
        if (!getResult.IsSuccess)
            return Result.Failure(getResult.Errors);
        
        if(getResult.Value.UserId != command.UserId)
            return Result.Failure(Result.ToDict("User", "Access denied"));

        var deleteResult = await _repository.Delete(command.Id,
            cancellationToken);
        if (!deleteResult.IsSuccess)
            return Result.Failure(deleteResult.Errors);
        
        return Result.Success();
    }
}