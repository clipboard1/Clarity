using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Clarity.Application;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResponse>> DispatchAsync<TQuery, TResponse>(TQuery query,
        CancellationToken cancellationToken = default) 
        where TQuery : IQuery<TResponse>
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        dynamic? handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException($"Handler for command {typeof(TQuery).Name} not found.");
        var validator = _serviceProvider.GetService<IValidator<TQuery>>();
        if (validator == null) 
            return await handler.Handle(query, cancellationToken);
        
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid) 
            return await handler.Handle(query, cancellationToken);
        
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