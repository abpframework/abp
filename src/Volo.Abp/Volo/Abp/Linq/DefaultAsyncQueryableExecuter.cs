using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.DependencyInjection;

namespace Volo.Abp.Linq
{
    //TODO: DefaultAsyncQueryableExecuter should be able to work with multiple Executer, each will try to execute it!
    //TODO: Implement with EF Core as first executer implementation!
    public class DefaultAsyncQueryableExecuter : IAsyncQueryableExecuter, ITransientDependency
    {
        public Task<int> CountAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.Count());
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.ToList());
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.FirstOrDefault());
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
        {
            return Task.FromResult(queryable.FirstOrDefault());
        }
    }
}