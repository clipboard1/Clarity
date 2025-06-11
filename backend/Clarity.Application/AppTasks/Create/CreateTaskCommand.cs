using System.Windows.Input;
using Clarity.Application.Abstractions;
using Clarity.Core.Enums;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.AppTasks.Create;

public record CreateTaskCommand(
    Guid UserId,
    string Title,
    string Description,
    DateTime CreationDate,
    DateTime Deadline,
    ICollection<Tag> Tags,
    AppTaskStatus Status)
    : ICommand<Guid>;
    
public class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, Guid>
{
    private readonly IAppTasksRepository _repository;

    public CreateTaskCommandHandler(IAppTasksRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateTaskCommand taskCommand)
    {
        var createResult = AppTask.Create(
            Guid.NewGuid(),
            taskCommand.UserId,
            taskCommand.Title,
            taskCommand.Description,
            taskCommand.CreationDate.ToUniversalTime(),
            taskCommand.Deadline.ToUniversalTime(),
            taskCommand.Tags,
            taskCommand.Status);
        if  (!createResult.IsSuccess)
            return Result<Guid>.Failure(createResult.Errors);

        var saveResult = await _repository.Create(
            taskCommand.UserId, createResult.Value);
        if (!saveResult.IsSuccess)
            return Result<Guid>.Failure(saveResult.Errors);
        
        return Result<Guid>.Success(saveResult.Value);
    }
}