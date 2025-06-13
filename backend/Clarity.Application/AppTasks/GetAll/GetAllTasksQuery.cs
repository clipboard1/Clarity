using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.AppTasks.GetAll;

public record GetAllTasksQuery(Guid UserId)
    : IQuery<List<AppTask>>;

public class GetAllTasksQueryHandler : IQueryHandler<GetAllTasksQuery, List<AppTask>>
{
    private readonly IAppTasksRepository _repository;

    public GetAllTasksQueryHandler(IAppTasksRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<AppTask>>> Handle(GetAllTasksQuery tasksQuery,
        CancellationToken cancellationToken = default)
    {
        var getResult = await _repository.GetAll(tasksQuery.UserId,
            cancellationToken);
        if (!getResult.IsSuccess)
            return Result<List<AppTask>>.Failure(getResult.Errors);
        
        return Result<List<AppTask>>.Success(getResult.Value);
    }
}