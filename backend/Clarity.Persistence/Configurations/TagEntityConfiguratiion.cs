using Clarity.Core.Models;
using Clarity.Persistence.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clarity.Persistence.Configurations;

public class TagEntityConfiguratiion : IEntityTypeConfiguration<TagEntity>
{
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Name)
            .IsRequired();
    }
}