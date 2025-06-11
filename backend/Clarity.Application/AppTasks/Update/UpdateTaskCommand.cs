using Clarity.Core.Models;
using Clarity.Application.Abstractions;
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
    
    public async Task<Result> Handle(UpdateTaskCommand taskCommand)
    {
        var findResult = await _repository.GetById(taskCommand.Id);
        if (!findResult.IsSuccess)
            return Result.Failure(findResult.Errors);
        
        var existingTask = findResult.Value;
        
        var newTitle = taskCommand.Update.Title ?? existingTask.Title;
        var newDescription = taskCommand.Update.Description ?? existingTask.Description;
        var newDeadline =  taskCommand.Update.Deadline ?? existingTask.Deadline;
        var newStatus = taskCommand.Update.Status ?? existingTask.Status;
        
        var createResult = AppTask.Create(
            existingTask.Id, taskCommand.Id,
            newTitle, newDescription, existingTask.CreationDate,
            newDeadline,  existingTask.Tags,
            newStatus
        );
        if (!createResult.IsSuccess)
            return Result.Failure(createResult.Errors);
        
        var updateResult = await _repository.Update(createResult.Value);
        if (!updateResult.IsSuccess)
            return Result.Failure(updateResult.Errors);
        
        return Result.Success();
    }
}