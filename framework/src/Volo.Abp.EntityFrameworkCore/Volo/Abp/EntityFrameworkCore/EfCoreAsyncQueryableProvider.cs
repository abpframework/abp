using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Linq;

namespace Volo.Abp.EntityFrameworkCore
{
    public class EfCoreAsyncQueryableProvider : IAsyncQueryableProvider, ITransientDependency
    {
        public bool CanExecute<T>(IQueryable<T> queryable)
        {
            return queryable.Provider is EntityQueryProvider;
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.CountAsync(cancellationToken);
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.ToListAsync(cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.FirstOrDefaultAsync(cancellationToken);
        }
    }
}