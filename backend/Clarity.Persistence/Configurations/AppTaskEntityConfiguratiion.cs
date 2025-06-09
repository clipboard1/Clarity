using Clarity.Core.Models;
using Clarity.Persistence.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clarity.Persistence.Configurations;

public class AppTaskEntityConfiguratiion : IEntityTypeConfiguration<AppTaskEntity>
{
    public void Configure(EntityTypeBuilder<AppTaskEntity> builder)
    {
        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Title)
            .HasMaxLength(AppTask.MAX_TITLE_LENGTH)
            .IsRequired();

        builder
            .Property(t => t.Description)
            .HasMaxLength(AppTask.MAX_DESCRIPTION_LENGTH)
            .IsRequired();

        builder
            .Property(t => t.CreationDate)
            .IsRequired();
        
        builder
            .Property(t => t.Deadline)
            .IsRequired();
        
        builder
            .Property(t => t.Status)
            .IsRequired();

        builder
            .HasMany(t => t.Tags)
            .WithMany(t => t.AppTasks)
            .UsingEntity<TaskTagEntity>(
                r => r.HasOne<TagEntity>()
                    .WithMany().HasForeignKey(t => t.TagId),
                l => l.HasOne<AppTaskEntity>()
                    .WithMany().HasForeignKey(t => t.AppTaskId));
    }
}