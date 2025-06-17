using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.Tags.GetAllByTask;

public record GetAllTagsByTaskQuery(
    Guid AppTaskId,
    Guid UserId)
    : IQuery<List<Tag>>;
    
public class GetAllTagsByTaskQueryHandler : IQueryHandler<GetAllTagsByTaskQuery, List<Tag>>
{
    private readonly ITagRepository _tagsRepository;
    private readonly IAppTasksRepository _tasksRepository; 
    
    public GetAllTagsByTaskQueryHandler(ITagRepository tagsRepository, IAppTasksRepository tasksRepository)
    {
        _tagsRepository = tagsRepository;
        _tasksRepository = tasksRepository;
    }

    public async Task<Result<List<Tag>>> Handle(GetAllTagsByTaskQuery query, 
        CancellationToken cancellationToken = default)
    {
        var getTaskResult = await _tasksRepository.GetById(query.AppTaskId, cancellationToken);
        if (!getTaskResult.IsSuccess)
            return Result<List<Tag>>.Failure(getTaskResult.Errors);
        
        if (getTaskResult.Value.UserId != query.UserId)
            return Result<List<Tag>>.Failure(Result.ToDict("User", "Access denied"));

        
        var getResult = await _tagsRepository.GetAllByTask(query.AppTaskId,
            cancellationToken);
        if (!getResult.IsSuccess)
            return Result<List<Tag>>.Failure(getResult.Errors);
        
        return Result<List<Tag>>.Success(getResult.Value);
    }
}