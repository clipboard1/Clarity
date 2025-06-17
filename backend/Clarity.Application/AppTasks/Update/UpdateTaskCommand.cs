using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.AppTasks.Update;

public record UpdateTaskCommand(
    Guid Id,
    Guid UserId,
    AppTaskUpdate Update)
    : ICommand;
    
public class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand>
{
    private readonly IAppTasksRepository _repository;

    public UpdateTaskCommandHandler(IAppTasksRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(UpdateTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        var findResult = await _repository.GetById(command.Id,
            cancellationToken);
        if (!findResult.IsSuccess)
            return Result.Failure(findResult.Errors);
        
        var existingTask = findResult.Value;
        
        if(existingTask.UserId != command.UserId)
            return Result.Failure(Result.ToDict("User", "Access denied"));
        
        var newTitle = command.Update.Title ?? existingTask.Title;
        var newDescription = command.Update.Description ?? existingTask.Description;
        var newDeadline =  command.Update.Deadline ?? existingTask.Deadline;
        var newStatus = command.Update.Status ?? existingTask.Status;

        if (newDeadline < existingTask.CreationDate)
            return Result.Failure(Result.ToDict("Deadline", "Deadline must be later than creation date"));
        
        var createResult = AppTask.Create(
            existingTask.Id, command.Id,
            newTitle, newDescription, existingTask.CreationDate,
            newDeadline,  existingTask.Tags,
            newStatus
        );
        if (!createResult.IsSuccess)
            return Result.Failure(createResult.Errors);
        
        var updateResult = await _repository.Update(createResult.Value,
            cancellationToken);
        if (!updateResult.IsSuccess)
            return Result.Failure(updateResult.Errors);
        
        return Result.Success();
    }
}