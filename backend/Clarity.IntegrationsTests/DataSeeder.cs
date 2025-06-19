using Clarity.Core.Abstractions;
using Clarity.Persistence;
using Clarity.Persistence.Entitites;

namespace Clarity.IntegrationsTests;

public class DataSeeder
{
    private readonly ClarityDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public DataSeeder(ClarityDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserEntity> AddUser()
    {
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = "testuser@example.com",
            Username = "testuser",
            PasswordHash = _passwordHasher.Generate("xxxtestpasswordxxx")
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<AppTaskEntity> AddTask()
    {
        var user = await AddUser();

        var task = new AppTaskEntity
        {
            Id = Guid.NewGuid(),
            Title = "Some title",
            Description = "Some description",
            UserId = user.Id
        };
        
        await _context.AppTasks.AddAsync(task);
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<TagEntity> AddTag()
    {
        var task = await AddTask();

        var tag = new TagEntity
        {
            AppTaskId = task.Id,
            Name = "Some name"
        };

        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        return tag;
    }
    
    public async Task<TagEntity> AddTag(Guid taskId)
    { 
        var tag = new TagEntity
        {
            AppTaskId = taskId,
            Name = "Some name"
        };

        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        return tag;
    }
}