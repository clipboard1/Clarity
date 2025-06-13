using Clarity.Core.Models;

namespace Clarity.Application.Abstractions;

public interface IQueryDispatcher
{
    Task<Result<TResponse>> DispatchAsync<TQuery, TResponse>(TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>;
}