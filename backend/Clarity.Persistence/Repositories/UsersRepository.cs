using Clarity.Core.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;
using Clarity.Persistence.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Clarity.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly ClarityDbContext _context;

    public UsersRepository(ClarityDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Add(User user, CancellationToken cancellationToken = default)
    {
        if (user.Id == Guid.Empty)
            return Result.Failure(Result.ToDict("UserId", "User id cannot be empty"));
        
        var existingUser = await _context.Users
            .AnyAsync(u => u.Id == user.Id, cancellationToken);
        
        if (existingUser)
            return Result.Failure(Result.ToDict("User","User already exists"));

        var userEntity = new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
        };
        
        try
        {
            await _context.Users.AddAsync(userEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(Result.ToDict("General",$"Failed to create user: " +
                                                         $"{ex.InnerException.Message ?? ex.Message}"));
        }
    }

    public async Task<Result<User>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            return Result<User>.Failure(Result.ToDict("UserId","User id cannot be empty"));

        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (userEntity == null)
            return Result<User>.Failure(Result.ToDict("User","User not found"));

        var result = User.Create(userEntity.Id, 
            userEntity.Username, 
            userEntity.Email, 
            userEntity.PasswordHash);

        if (!result.IsSuccess)
            return Result<User>.Failure(result.Errors);

        return Result<User>.Success(result.Value);
    }

    public async Task<Result<User>> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(email))
            return Result<User>.Failure(Result.ToDict("Email","User email cannot be empty"));

        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (userEntity == null)
            return Result<User>.Failure(Result.ToDict("User","User not found"));

        var result = User.Create(userEntity.Id, 
            userEntity.Username, 
            userEntity.Email, 
            userEntity.PasswordHash);

        if (!result.IsSuccess)
            return Result<User>.Failure(result.Errors);

        return Result<User>.Success(result.Value);
    }

    public async Task<Result> SaveRefreshToken(Guid userId, string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var refreshToken = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = token,
                ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
            };
    
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(Result.ToDict("General", $"Failed to save token: " +
                                  $"{ex.InnerException.Message ?? ex.Message}"));
        }
    }

    public async Task<Result<RefreshTokenEntity>> GetRefreshToken(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var refreshToken = await
                _context.RefreshTokens
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.Token == token,
                        cancellationToken);
        
            if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
                return Result<RefreshTokenEntity>.Failure(Result.ToDict("RefreshToken", "Invalid refresh token"));
        
            return Result<RefreshTokenEntity>.Success(refreshToken);
        }
        catch (DbUpdateException ex)
        {
            return Result<RefreshTokenEntity>.Failure(Result.ToDict("General", $"Failed to create token: " +
                                                                    $"{ex.InnerException.Message ?? ex.Message}"));
        }
    }

    public async Task<Result> UpdateRefreshToken(RefreshTokenEntity refreshToken, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Update(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result<RefreshTokenEntity>.Failure(Result.ToDict("General", $"Failed to create token: " + 
                                                                               $"{ex.InnerException.Message ?? ex.Message}"));
        }
    }
}