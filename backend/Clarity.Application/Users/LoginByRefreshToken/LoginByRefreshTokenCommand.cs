using Clarity.Application.Abstractions;
using Clarity.Core.Abstractions;
using Clarity.Core.Dto;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.Users.LoginByRefreshToken;

public record LoginByRefreshTokenCommand(
    string RefreshToken)
    : ICommand<AuthTokens>;
    
public class LoginByRefreshTokenCommandHandler : ICommandHandler<LoginByRefreshTokenCommand, AuthTokens>
{
    private readonly IUsersRepository _repository;
    private readonly IJwtProvider _jwtProvider;

    public LoginByRefreshTokenCommandHandler(IUsersRepository repository, IJwtProvider jwtProvider)
    {
        _repository = repository;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<AuthTokens>> Handle(LoginByRefreshTokenCommand command)
    {
        var existingRefreshToken = await _repository.GetRefreshToken(command.RefreshToken);
        if (!existingRefreshToken.IsSuccess)
            return Result<AuthTokens>.Failure(existingRefreshToken.Errors);
        
        var userEntity = existingRefreshToken.Value.User;
        var user = User.Create(
            userEntity.Id,
            userEntity.Username,
            userEntity.Email,
            userEntity.PasswordHash
            ).Value;
        
        var accesstokenGenResult = _jwtProvider.GenerateAuthToken(user);
        if (!accesstokenGenResult.IsSuccess)
            return Result<AuthTokens>.Failure(accesstokenGenResult.Errors);
        
        existingRefreshToken.Value.Token = _jwtProvider.GenereateRefreshToken().Value;
        existingRefreshToken.Value.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);
        
        await _repository.UpdateRefreshToken(existingRefreshToken.Value);

        var tokens = new AuthTokens
        {
            AuthToken = accesstokenGenResult.Value,
            RefreshToken = existingRefreshToken.Value.Token
        };
        
        return Result<AuthTokens>.Success(tokens);
    }
}