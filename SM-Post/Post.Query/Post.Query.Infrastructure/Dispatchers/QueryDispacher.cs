using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.Dispatchers;
public sealed class QueryDispacher : IQueryDispacher<PostEntity>
{
    private readonly Dictionary<Type, Func<QueryBase, Task<List<PostEntity>>>> _handlers = [];
    public void RegisterAsync<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler) where TQuery : QueryBase
    {
        if (_handlers.ContainsKey(typeof(TQuery)))
        {
            throw new InvalidOperationException($"Handler for query {typeof(TQuery).Name} already registered.");
        }
        _handlers.Add(typeof(TQuery), query => handler((TQuery)query));
    }

    public async Task<List<PostEntity>> SendAsync<TQuery>(TQuery query) where TQuery : QueryBase
    {
        if (!_handlers.ContainsKey(typeof(TQuery)))
        {
            throw new InvalidOperationException($"Handler for query {typeof(TQuery).Name} not registered.");
        }
        return await _handlers[typeof(TQuery)](query);
    }
}
