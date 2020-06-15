using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Volo.Abp.MongoDB
{
    public class MongoDbAsyncQueryableProvider : IAsyncQueryableProvider, ITransientDependency
    {
        public bool CanExecute<T>(IQueryable<T> queryable)
        {
            return queryable.Provider.GetType().Namespace?.StartsWith("MongoDB") ?? false;
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return ((IMongoQueryable<T>) queryable).CountAsync(cancellationToken);
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return ((IMongoQueryable<T>) queryable).ToListAsync(cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return ((IMongoQueryable<T>) queryable).FirstOrDefaultAsync(cancellationToken);
        }
    }
}