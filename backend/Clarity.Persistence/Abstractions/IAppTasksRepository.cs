using Clarity.Core.Models;

namespace Clarity.Persistence.Abstractions;

public interface IAppTasksRepository
{
    Task<Result<List<AppTask>>> GetAll(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<AppTask>> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Guid>> Create(Guid userId, AppTask task, CancellationToken cancellationToken = default);
    Task<Result> Update(AppTask task, CancellationToken cancellationToken = default);
    Task<Result> Delete(Guid id, CancellationToken cancellationToken = default);
}