using Clarity.Persistence;
using Clarity.Persistence.Entitites;

namespace Clarity.IntegrationsTests;

public class DataSeeder
{
    private readonly ClarityDbContext _context;

    public DataSeeder(ClarityDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity> AddUser()
    {
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = "testuser@example.com",
            Username = "testuser",
            PasswordHash = "xxxtestpasswordxxx"
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
}