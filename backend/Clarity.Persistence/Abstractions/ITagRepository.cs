using Clarity.Core.Models;

namespace Clarity.Persistence.Abstractions;

public interface ITagRepository
{
    Task<Result<List<Tag>>> GetAllByTask(Guid taskId, CancellationToken cancellationToken = default);
    Task<Result<int>> Create(Tag tag, CancellationToken cancellationToken = default);
    Task<Result> Delete(int id, CancellationToken cancellationToken = default);
}