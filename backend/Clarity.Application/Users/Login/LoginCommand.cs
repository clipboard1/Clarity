using Clarity.Application.Abstractions;
using Clarity.Core.Abstractions;
using Clarity.Core.Dto;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.Users.Login;

public record LoginCommand(
    string Email, 
    string Password)
    : ICommand<AuthTokens>;

public class LoginCommandHandler : ICommandHandler<LoginCommand, AuthTokens>
{
    private readonly IUsersRepository _repository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(IUsersRepository repository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthTokens>> Handle(LoginCommand command)
    {
        var getResult = await _repository.GetByEmail(command.Email);
        if (!getResult.IsSuccess)
            return Result<AuthTokens>.Failure(getResult.Errors);
        
        var verifyResult = _passwordHasher.Verify(
            command.Password, 
            getResult.Value.PasswordHash);
        if (!verifyResult)
            return Result<AuthTokens>.Failure(Result.ToDict("Password", "Wrong password"));
        
        var generateTokenResult = _jwtProvider.GenerateAuthToken(getResult.Value);
        if (!generateTokenResult.IsSuccess)
            return Result<AuthTokens>.Failure(generateTokenResult.Errors);
        
        var generateRefreshTokenResult = _jwtProvider.GenereateRefreshToken();
        if (!generateRefreshTokenResult.IsSuccess)
            return Result<AuthTokens>.Failure(generateRefreshTokenResult.Errors);

        await _repository.SaveRefreshToken(
            getResult.Value.Id,
            generateRefreshTokenResult.Value);

        var tokens = new AuthTokens
        {
            AuthToken = generateTokenResult.Value,
            RefreshToken = generateRefreshTokenResult.Value
        };
        
        return Result<AuthTokens>.Success(tokens);
    }
}