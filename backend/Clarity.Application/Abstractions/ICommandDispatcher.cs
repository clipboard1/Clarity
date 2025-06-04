using Clarity.Core.Models;

namespace Clarity.Application.Abstractions;
public interface ICommandDispatcher
{
    Task<Result> DispatchAsync<TCommand>(TCommand command)
        where TCommand : ICommand;

    Task<Result<TResponse>> DispatchAsync<TCommand, TResponse>(TCommand command)
        where TCommand : ICommand<TResponse>;
}