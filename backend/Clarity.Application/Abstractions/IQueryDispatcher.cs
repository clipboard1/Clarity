using Clarity.Core.Models;

namespace Clarity.Application.Abstractions;

public interface IQueryDispatcher
{
    Task<Result<TResponse>> DispatchAsync<TQuery, TResponse>(TQuery query)
        where TQuery : IQuery<TResponse>;
}