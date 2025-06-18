using Clarity.Core.Enums;
using Clarity.Core.Models;

namespace Clarity.Persistence.Entitites;

public class AppTaskEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public AppTaskStatus Status { set; get; }
    
    public virtual UserEntity? User { get; set; }
    public virtual ICollection<TagEntity> Tags { get; set; } = null!;
}