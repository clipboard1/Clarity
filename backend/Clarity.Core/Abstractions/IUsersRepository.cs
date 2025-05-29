using Clarity.Core.Models;

namespace Clarity.Core.Abstractions;

public interface IUsersRepository
{
    Task<Result> Add(User user, CancellationToken cancellationToken = default);
    Task <Result<User>> GetById(Guid id, CancellationToken cancellationToken = default);
    Task <Result<User>> GetByEmail(string email, CancellationToken cancellationToken = default);
}