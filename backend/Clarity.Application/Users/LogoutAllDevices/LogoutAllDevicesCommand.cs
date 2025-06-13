using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.LogoutAllDevices;

public record LogoutAllDevicesCommand(
    Guid UserId)
    : ICommand;
    
public class LogoutAllDevicesCommandHandler : ICommandHandler<LogoutAllDevicesCommand>
{
    private readonly IUsersRepository _repository;

    public LogoutAllDevicesCommandHandler(IUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(LogoutAllDevicesCommand command,
        CancellationToken cancellationToken = default)
    {
        var revokeResult = await _repository.RevokeAllRefreshTokens(command.UserId,
            cancellationToken);
        if (!revokeResult.IsSuccess)
            return Result.Failure(revokeResult.Errors);
        
        return Result.Success();
    }
}