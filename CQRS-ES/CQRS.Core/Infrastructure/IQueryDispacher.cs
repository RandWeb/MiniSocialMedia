using CQRS.Core.Queries;

namespace CQRS.Core.Infrastructure;
public interface IQueryDispacher<TEntity>
{
    void RegisterAsync<TQuery>(Func<TQuery,Task<List<TEntity>>> handler)  where TQuery : QueryBase;
    Task<List<TEntity>> SendAsync<TQuery>(TQuery query) where TQuery : QueryBase;
}
