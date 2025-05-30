using Clarity.Core.Models;

namespace Clarity.Persistence.Entitites;

public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    
    public virtual UserEntity User { get; set; } = null!;
}