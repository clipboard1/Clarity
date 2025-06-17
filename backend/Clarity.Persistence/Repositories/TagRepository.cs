using AutoMapper;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;
using Clarity.Persistence.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Clarity.Persistence.Repositories;

public class TagRepository : ITagRepository
{
    private readonly ClarityDbContext _context;
    private readonly IMapper _mapper;

    public TagRepository(ClarityDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<Tag>>> GetAllByTask(Guid taskId, CancellationToken cancellationToken = default)
    {
        if (taskId == Guid.Empty)
            return Result<List<Tag>>.Failure(Result.ToDict("TaskId", "Task id cannot be empty"));

        var tags = await _context.Tags
            .Where(t => t.AppTaskId == taskId)
            .ToListAsync(cancellationToken);
        
        return Result<List<Tag>>.Success(_mapper.Map<List<Tag>>(tags));
    }

    public async Task<Result<int>> Create(Tag tag, CancellationToken cancellationToken = default)
    {
        var existingTag = await _context.Tags
            .AnyAsync(t =>
                t.Name == tag.Name &&
                t.AppTaskId == tag.AppTaskId,
                cancellationToken);
        if (existingTag)
            return Result<int>.Failure(Result.ToDict("Tag", "Tag with this name already exist"));

        var tagEntity = new TagEntity
        {
            Name = tag.Name,
            AppTaskId = tag.AppTaskId
        };
        
        try
        {
            await _context.Tags.AddAsync(tagEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(tagEntity.Id);
        }
        catch (DbUpdateException ex)
        {
            return Result<int>.Failure(Result.ToDict("General", $"Failed to create tag: " +
                                                                 $"{ex.InnerException.Message ?? ex.Message}"));
        }
    }

    public async Task<Result> Delete(int id, CancellationToken cancellationToken = default)
    {
        if (id == 0)
            return Result.Failure(Result.ToDict("Tag", "Tag id cannot be empty"));
        
        try
        {
            var deletedRows = await _context.Tags
                .Where(t => 
                    t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRows == 0)
                return Result.Failure(Result.ToDict("Tag", "Tag not found."));

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(Result.ToDict("General", $"Failed to delete tag: " +
                                                                 $"{ex.InnerException.Message ?? ex.Message}"));
        }
    }
}