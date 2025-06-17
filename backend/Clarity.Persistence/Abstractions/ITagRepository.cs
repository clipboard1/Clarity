using Clarity.Core.Models;

namespace Clarity.Persistence.Abstractions;

public interface ITagRepository
{
    Task<Result<List<Tag>>> GetAllByTask(Guid taskId, CancellationToken cancellationToken);
    Task<Result<int>> Create(Tag tag, CancellationToken cancellationToken);
    Task<Result> Delete(int id, CancellationToken cancellationToken);
}