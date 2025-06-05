using Clarity.Core.Models;
using Clarity.Persistence.Entitites;

namespace Clarity.Persistence.Abstractions;

public interface IUsersRepository
{
    Task<Result> Add(User user, CancellationToken cancellationToken = default);
    Task <Result<User>> GetById(Guid id, CancellationToken cancellationToken = default);
    Task <Result<User>> GetByEmail(string email, CancellationToken cancellationToken = default);
    
    Task<Result> SaveRefreshToken(Guid userId, string token, CancellationToken cancellationToken = default);
    Task<Result<RefreshTokenEntity>> GetRefreshToken(string token, CancellationToken cancellationToken = default);
    Task <Result> UpdateRefreshToken(RefreshTokenEntity newRefreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshToken(string token, CancellationToken cancellationToken = default);
    Task<Result> RevokeAllRefreshTokens(Guid userId, CancellationToken cancellationToken = default);
}