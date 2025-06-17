using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Clarity.Application;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Result> DispatchAsync<TCommand>(TCommand command,
        CancellationToken cancellationToken = default) 
        where TCommand : ICommand
    {
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
        if (handler == null)
            throw new InvalidOperationException($"Handler for command {typeof(TCommand).Name} not found.");
        var validator = _serviceProvider.GetService<IValidator<TCommand>>();
        if (validator == null) 
            return await handler.Handle(command, cancellationToken);
        
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid) 
            return await handler.Handle(command, cancellationToken);
        
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                el => el.Key,
                el =>
                    el.Select(p => p.ErrorMessage)
                        .ToArray());
        return Result.Failure(errors);

    }
    
    public async Task<Result<TResponse>> DispatchAsync<TCommand, TResponse>(TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        dynamic? handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException($"Handler for command {typeof(TCommand).Name} not found.");
        var validator = _serviceProvider.GetService<IValidator<TCommand>>();
        if (validator == null) 
            return await handler.Handle(command, cancellationToken);
        
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid) 
            return await handler.Handle(command, cancellationToken);
        
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                el => el.Key,
                el =>
                    el.Select(p => p.ErrorMessage)
                        .ToArray());
        return Result<TResponse>.Failure(errors);
    }
}
