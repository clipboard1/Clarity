using Clarity.Core.Models;

namespace Clarity.Application.Abstractions;

public interface ICommandDispatcher
{
    Task<Result> DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
}

public interface ICommandDispatcher<TResponse>
{
    Task<Result<TResponse>> DispatchAsync<TCommand>(TCommand command) where TCommand 
        : ICommand<TResponse>;
}