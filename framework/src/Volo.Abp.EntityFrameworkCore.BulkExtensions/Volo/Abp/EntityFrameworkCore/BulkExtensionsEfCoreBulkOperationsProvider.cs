using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.BulkExtensions.Volo.Abp.EntityFrameworkCore
{
    public class BulkExtensionsEfCoreBulkOperationsProvider : IEfCoreBulkOperationProvider, ITransientDependency
    {
        public Task DeleteManyAsync<TDbContext, TEntity>(IEfCoreRepository<TEntity> repository,
                                                            IEnumerable<TEntity> entities,
                                                            bool autoSave,
                                                            CancellationToken cancellationToken)
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
        {
            return repository.DbContext.BulkDeleteAsync(entities.ToList());
        }

        public Task InsertManyAsync<TDbContext, TEntity>(IEfCoreRepository<TEntity> repository,
                                                                IEnumerable<TEntity> entities,
                                                                bool autoSave,
                                                                CancellationToken cancellationToken)
            where TDbContext : IEfCoreDbContext
            where TEntity : class, IEntity
        {
            return repository.DbContext.BulkInsertAsync(entities.ToList());
        }

        public Task UpdateManyAsync<TDbContext, TEntity>(IEfCoreRepository<TEntity> repository,
                                                                IEnumerable<TEntity> entities,
                                                                bool autoSave,
                                                                CancellationToken cancellationToken)
            where TDbContext : IEfCoreDbContext
            where TEntity : class, IEntity
        {
            return repository.DbContext.BulkUpdateAsync(entities.ToList());
        }
    }
}
