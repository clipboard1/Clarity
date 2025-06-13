using Clarity.Core.Models;

namespace Clarity.Application.Abstractions;
public interface ICommandDispatcher
{
    Task<Result> DispatchAsync<TCommand>(TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand;

    Task<Result<TResponse>> DispatchAsync<TCommand, TResponse>(TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>;
}