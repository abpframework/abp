using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public interface IMongoDbBulkOperationProvider
    {
        Task InsertManyAsync<TEntity>(
           IMongoDbRepository<TEntity> repository,
           IEnumerable<TEntity> entities,
            IClientSessionHandle sessionHandle,
           bool autoSave,
           CancellationToken cancellationToken
       )
           where TEntity : class, IEntity;

        Task UpdateManyAsync<TEntity>(
            IMongoDbRepository<TEntity> repository,
            IEnumerable<TEntity> entities,
            IClientSessionHandle sessionHandle,
            bool autoSave,
            CancellationToken cancellationToken
        )
            where TEntity : class, IEntity;

        Task DeleteManyAsync<TEntity>(
            IMongoDbRepository<TEntity> repository,
            IEnumerable<TEntity> entities,
            IClientSessionHandle sessionHandle,
            bool autoSave,
            CancellationToken cancellationToken
        )
            where TEntity : class, IEntity;
    }
}
