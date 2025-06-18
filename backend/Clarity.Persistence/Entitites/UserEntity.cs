namespace Clarity.Persistence.Entitites;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<AppTaskEntity> AppTasks { get; set; } = null!;
}