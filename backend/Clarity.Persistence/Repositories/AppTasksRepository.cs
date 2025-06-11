using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;
using Clarity.Persistence.Entitites;
using Clarity.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Clarity.Persistence.Repositories;

public class AppTasksRepository : IAppTasksRepository
{
    private readonly ClarityDbContext _context;

    public AppTasksRepository(ClarityDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<AppTask>>> GetAll(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            return Result<List<AppTask>>.Failure(Result.ToDict("UserId", "User id cannot be empty"));

        var tasks = await _context.AppTasks
            .Where(t => t.UserId == userId)
            .Include(t => t.Tags)
            .ToListAsync(cancellationToken);

        var domainTasks = 
            tasks.Select(t => t.ToDomainModel())
                .ToList();

        return Result<List<AppTask>>.Success(domainTasks);
    }

    public async Task<Result<AppTask>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            return Result<AppTask>.Failure(Result.ToDict("TaskId", "Task id cannot be empty"));

        var task = await _context.AppTasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        if (task is null)
            return Result<AppTask>.Failure(Result.ToDict("Task", "Task not found"));

        var domainTask =
            task.ToDomainModel();
        
        return Result<AppTask>.Success(domainTask);
    }

    public async Task<Result<Guid>> Create(Guid userId, AppTask task, CancellationToken cancellationToken = default)
    {
        if (task.Id == Guid.Empty)
            return Result<Guid>.Failure(Result.ToDict("Task id", "Id cannot be empty."));
        
        var existingTask = await _context.AppTasks.AnyAsync(t => 
            t.UserId == userId &&
            t.Id == task.Id);
        
        if (existingTask)
            return Result<Guid>.Failure(Result.ToDict("Task", "Task with this id already exists."));
        
        var taskEntity = new AppTaskEntity
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            CreationDate = task.CreationDate,
            Deadline = task.Deadline,
            Status = task.Status,
            UserId = userId
        };
        
        try
        {
            await _context.AppTasks.AddAsync(taskEntity);
            await _context.SaveChangesAsync();
            return Result<Guid>.Success(taskEntity.Id);
        }
        catch (DbUpdateException ex)
        {
            return Result<Guid>.Failure(Result.ToDict("General", $"Failed to create task: " +
                                        $"{ex.InnerException.Message ?? ex.Message}"));
        }
    }

    public async Task<Result> Update(AppTask task, CancellationToken cancellationToken = default)
    {
        if (task.Id == Guid.Empty)
            return Result.Failure(Result.ToDict("TaskId", "Id cannot be empty."));

        try
        {
            var updateRows = await _context.AppTasks
                .Where(t => 
                    t.Id == task.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.Title, task.Title)
                    .SetProperty(t => t.Description, task.Description)
                    .SetProperty(t => t.Deadline, task.Deadline)
                    .SetProperty(t => t.Status, task.Status),
                    cancellationToken);
            
            if (updateRows == 0)
                return Result.Failure(Result.ToDict("Task", "Task not found."));
            
            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result<Guid>.Failure(Result.ToDict("General", $"Failed to create task: " +
                                                                 $"{ex.InnerException.Message ?? ex.Message}"));
        }
    }

    public async Task<Result> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            return Result.Failure(Result.ToDict("TaskId", "Id cannot be empty."));

        try
        {
            var deletedRows = await _context.AppTasks
                .Where(t => 
                    t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRows == 0)
                return Result.Failure(Result.ToDict("Task", "Task not found."));

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result<Guid>.Failure(Result.ToDict("General", $"Failed to delete task: " +
                                                                 $"{ex.InnerException.Message ?? ex.Message}"));
        }
    }

    public async Task<Result> ChangeStatus(Guid id, int status, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}