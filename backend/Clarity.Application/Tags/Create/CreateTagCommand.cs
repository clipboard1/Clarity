using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.Tags.Create;

public record CreateTagCommand(
    string Name,
    Guid AppTaskId,
    Guid UserId)
    : ICommand<int>;
    
public class CreateTagCommandHandler : ICommandHandler<CreateTagCommand, int>
{
    private readonly ITagRepository _tagsRepository;
    private readonly IAppTasksRepository _tasksRepository; 

    public CreateTagCommandHandler(ITagRepository tagsRepository, IAppTasksRepository tasksRepository)
    {
        _tagsRepository = tagsRepository;
        _tasksRepository = tasksRepository;
    }

    public async Task<Result<int>> Handle(CreateTagCommand command, 
        CancellationToken cancellationToken = default)
    {
        var getTaskResult = await _tasksRepository.GetById(command.AppTaskId, cancellationToken);
        if (!getTaskResult.IsSuccess)
            return Result<int>.Failure(getTaskResult.Errors);
        
        if (getTaskResult.Value.UserId != command.UserId)
            return Result<int>.Failure(Result.ToDict("User", "Access denied"));
        
        var createResult = Tag.Create(
            0,
            command.Name,
            command.AppTaskId);
        if  (!createResult.IsSuccess)
            return Result<int>.Failure(createResult.Errors);

        var saveResult = await _tagsRepository.Create(
            createResult.Value,
            cancellationToken);
        if (!saveResult.IsSuccess)
            return Result<int>.Failure(saveResult.Errors);
        
        return Result<int>.Success(saveResult.Value);
    }
}