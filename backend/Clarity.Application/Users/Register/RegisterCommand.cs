using Clarity.Application.Abstractions;
using Clarity.Core.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.Users.Register;

public record RegisterCommand(
    string Username, 
    string Email, 
    string Password)
    : ICommand;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IUsersRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IUsersRepository repository,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Username))
            return Result.Failure(Result.ToDict("Username", "Username is required"));

        if (string.IsNullOrEmpty(command.Email))
            return Result.Failure(Result.ToDict("Email", "Email is required"));

        if (string.IsNullOrEmpty(command.Password))
            return Result.Failure(Result.ToDict("Password", "Password is required"));
            
        
        string hashedPassword = _passwordHasher.Generate(command.Password);
        
        var createResult = User.Create(
            Guid.NewGuid(),
            command.Username, 
            command.Email, 
            hashedPassword);
        
        if (!createResult.IsSuccess)
            return Result.Failure(createResult.Errors);
        
        var saveResult = await _repository.Add(createResult.Value,
            cancellationToken);
        
        if (!saveResult.IsSuccess)
            return Result.Failure(saveResult.Errors);
        
        return Result.Success();
    }
}