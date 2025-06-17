namespace Clarity.Core.Models;

public class Tag
{
    public const int MAX_NAME_LENGTH = 16;
    public const int MIN_NAME_LENGTH = 1;
    
    public int Id { get; set; }
    public string Name { get; set; }
    public Guid AppTaskId { get; set; }

    private Tag(int id, string name, Guid appTaskId)
    {
        Id = id;
        Name = name;
        AppTaskId = appTaskId;
    }

    public Result<Tag> Create(int id, string name, Guid appTaskId)
    {
        var errors = new Dictionary<string, string[]>();
        
        if (appTaskId == Guid.Empty)
            errors.Add("Id", [$"Id must be not empty."]);
        
        if (string.IsNullOrEmpty(name) || name.Length < MIN_NAME_LENGTH)
            errors.Add("Name", [$"Name must be not null and at least {MIN_NAME_LENGTH} characters."]);
        else if (name.Length > MIN_NAME_LENGTH)
            errors.Add("Name", [$"Name must be less than {MAX_NAME_LENGTH} characters."]);
        
        if (errors.Any())
            return Result<Tag>.Failure(errors);

        var tag = new Tag(id, name, appTaskId);
        
        return Result<Tag>.Success(tag);
    }
}