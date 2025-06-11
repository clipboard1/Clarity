using Clarity.Core.Models;
using Clarity.Persistence.Entitites;

namespace Clarity.Persistence.Extensions;

public static class AppTaskEntityExtension
{
    public static AppTask ToDomainModel(this AppTaskEntity entity)
        => AppTask.Create(entity.Id, entity.UserId, entity.Title, 
            entity.Description,entity.CreationDate, 
            entity.Deadline, entity.Tags.Select(tt => tt.ToDomainModel()).ToList(), 
            entity.Status).Value;
    
    public static Tag ToDomainModel(this TagEntity entity)
        => new Tag { Id = entity.Id, Name = entity.Name, AppTaskId = entity.AppTaskId };

}