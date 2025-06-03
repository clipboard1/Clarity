using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Clarity.Application;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Result> DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
        if (handler == null)
            throw new InvalidOperationException($"Handler for command {typeof(TCommand).Name} not found.");

        return await handler.Handle(command);
    }
}

public class CommandDispatcher<TResponse> : ICommandDispatcher<TResponse>
{
    private readonly IServiceProvider _serviceProvider;
    
    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Result<TResponse>> DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand<TResponse>
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        dynamic handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException($"Handler for command {typeof(TCommand).Name} not found.");

        return await handler.Handle(command);
    }
}