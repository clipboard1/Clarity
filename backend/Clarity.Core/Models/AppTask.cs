using Clarity.Core.Enums;

namespace Clarity.Core.Models;

public class AppTask
{
    public const int MAX_TITLE_LENGTH = 128;
    public const int MAX_DESCRIPTION_LENGTH = 1024;
    public const int MIN_TITLE_LENGTH = 1;
    
    private AppTask(Guid id, Guid userId, string title, 
        string description,DateTime creationDate, DateTime deadline, 
        AppTaskStatus status, ICollection<Tag> tags)
    {
        Id = id;
        UserId = userId;
        Title = title;
        Description = description;
        CreationDate = creationDate;
        Deadline = deadline;
        Status = status;
        Tags = tags;
    }
    
    public Guid Id { get; }
    public Guid UserId { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime CreationDate { get; }
    public DateTime Deadline { get; }
    public AppTaskStatus Status { set; get; }
    public ICollection<Tag> Tags { get; }
    
    public static Result<AppTask> Create(Guid id, Guid userId, string title,
        string description, DateTime creationDate, DateTime deadline,
        ICollection<Tag> tags,
        AppTaskStatus status = AppTaskStatus.NotStarted)
    {
        var errors = new Dictionary<string, string[]>();

        if (id == Guid.Empty)
            errors.Add("Id", [$"Id must be not empty."]);
        
        if (userId == Guid.Empty)
            errors.Add("Id", [$"User id must be not empty."]);
            
        if (string.IsNullOrEmpty(title) || title.Length < MIN_TITLE_LENGTH)
            errors.Add("Title", [$"Title must be not null and at least {MIN_TITLE_LENGTH} characters."]);

        else if (title.Length > MAX_TITLE_LENGTH)
            errors.Add("Title", [$"Title must be less than {MAX_TITLE_LENGTH} characters."]);

        if (description?.Length > MAX_DESCRIPTION_LENGTH)
            errors.Add("Description", [$"Description must be less than {MAX_DESCRIPTION_LENGTH} characters."]);

        if (deadline < creationDate)
            errors.Add("Deadline", ["Deadline must be greater than creation date"]);
        
        if (errors.Any())
            return Result<AppTask>.Failure(errors);

        var task = new AppTask(id, userId, title, 
            description ?? string.Empty, 
            creationDate, deadline, status, tags);

        return Result<AppTask>.Success(task);

    }

    public Result ChangeStatus(AppTaskStatus newStatus)
    {
        Status = newStatus;
        return Result.Success();
    }
}