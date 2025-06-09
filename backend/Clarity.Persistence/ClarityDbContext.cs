using Clarity.Core.Models;
using Clarity.Persistence.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Clarity.Persistence;

public class ClarityDbContext(DbContextOptions<ClarityDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<AppTaskEntity> AppTasks { get; set; }
    public DbSet<TagEntity> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClarityDbContext).Assembly);
    }
}