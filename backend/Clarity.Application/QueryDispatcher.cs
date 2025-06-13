using Clarity.Application.Abstractions;
using Clarity.Core.Models;

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
        dynamic handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException($"Handler for command {typeof(TQuery).Name} not found.");

        return await handler.Handle(query, cancellationToken);
    }
}