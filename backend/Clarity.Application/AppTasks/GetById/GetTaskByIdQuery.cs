using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.AppTasks.GetById;

public record GetTaskByIdQuery(Guid Id)
    : IQuery<AppTask>;
    
public class GetTaskByIdQueryHandler : IQueryHandler<GetTaskByIdQuery, AppTask>
{
    private readonly IAppTasksRepository _repository;

    public GetTaskByIdQueryHandler(IAppTasksRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AppTask>> Handle(GetTaskByIdQuery query)
    {
        var getResult = await _repository.GetById(query.Id);
        if (!getResult.IsSuccess)
            return Result<AppTask>.Failure(getResult.Errors);
        
        return Result<AppTask>.Success(getResult.Value);
    }
}