using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Linq
{
    public interface IAsyncQueryableProvider
    {
        bool CanExecute<T>(IQueryable<T> queryable);
        
        Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);
    }
}