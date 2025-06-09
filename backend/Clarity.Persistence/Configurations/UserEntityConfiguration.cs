using Clarity.Core.Models;
using Clarity.Persistence.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clarity.Persistence.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .HasKey(u => u.Id);
        
        builder
            .Property(u => u.Username)
            .HasMaxLength(User.MAX_USERNAME_LENGTH)
            .IsRequired();
        
        builder
            .Property(u => u.Email)
            .IsRequired();
        
        builder
            .Property(u => u.PasswordHash)
            .IsRequired();


        builder
            .HasMany(u => u.AppTasks)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}