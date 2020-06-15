using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Linq
{
    public class AsyncQueryableExecuter : IAsyncQueryableExecuter, ITransientDependency
    {
        protected IEnumerable<IAsyncQueryableProvider> Providers { get; }

        public AsyncQueryableExecuter(IEnumerable<IAsyncQueryableProvider> providers)
        {
            Providers = providers;
        }

        public virtual Task<int> CountAsync<T>(
            IQueryable<T> queryable, 
            CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.CountAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Count());
        }

        public virtual Task<List<T>> ToListAsync<T>(
            IQueryable<T> queryable,
            CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.ToListAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.ToList());
        }

        public virtual Task<T> FirstOrDefaultAsync<T>(
            IQueryable<T> queryable,
            CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.FirstOrDefaultAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.FirstOrDefault());
        }

        protected virtual IAsyncQueryableProvider FindProvider<T>(IQueryable<T> queryable)
        {
            return Providers.FirstOrDefault(p => p.CanExecute(queryable));
        }
    }
}