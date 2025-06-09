namespace Clarity.Persistence.Entitites;

public class TagEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Guid AppTaskId { get; set; }
    
    public virtual ICollection<AppTaskEntity>? AppTasks { get; set; }
}