using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.Users.Logout;

public record LogoutCommand(
    string Token)
    : ICommand;
    
public class LogoutCommandHandler: ICommandHandler<LogoutCommand>
{
    
    private readonly IUsersRepository _repository;

    public LogoutCommandHandler(IUsersRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(LogoutCommand command,
        CancellationToken cancellationToken = default)
    {
        var revokeResult = await _repository.RevokeRefreshToken(command.Token,
            cancellationToken);
        if (!revokeResult.IsSuccess)
            return Result.Failure(revokeResult.Errors);
        
        return Result.Success();
    }
}