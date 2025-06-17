using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.AppTasks.GetById;

public record GetTaskByIdQuery(
    Guid Id,
    Guid UserId)
    : IQuery<AppTask>;
    
public class GetTaskByIdQueryHandler : IQueryHandler<GetTaskByIdQuery, AppTask>
{
    private readonly IAppTasksRepository _repository;

    public GetTaskByIdQueryHandler(IAppTasksRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AppTask>> Handle(GetTaskByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var getResult = await _repository.GetById(query.Id,
            cancellationToken);
        if (!getResult.IsSuccess)
            return Result<AppTask>.Failure(getResult.Errors);
        
        if (getResult.Value.UserId != query.UserId)
            return Result<AppTask>.Failure(Result.ToDict("User", "Access denied"));
        
        return Result<AppTask>.Success(getResult.Value);
    }
}