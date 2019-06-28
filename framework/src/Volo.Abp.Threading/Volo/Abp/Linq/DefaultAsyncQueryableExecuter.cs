using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Linq
{
    public class DefaultAsyncQueryableExecuter : IAsyncQueryableExecuter
    {
        public static DefaultAsyncQueryableExecuter Instance { get; } = new DefaultAsyncQueryableExecuter();

        private DefaultAsyncQueryableExecuter()
        {
            
        }

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