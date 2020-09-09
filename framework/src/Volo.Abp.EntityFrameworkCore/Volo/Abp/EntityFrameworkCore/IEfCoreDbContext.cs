using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Volo.Abp.EntityFrameworkCore
{
    public interface IEfCoreDbContext : IDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable
    {
        EntityEntry<TEntity> Attach<TEntity>([NotNull] TEntity entity) where TEntity : class;

        EntityEntry Attach([NotNull] object entity);

        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This method will call the DbContext <see cref="SaveChangesAsync(bool, CancellationToken)"/> method directly of EF Core, which doesn't apply concepts of abp.
        /// </summary>
        Task<int> SaveChangesOnDbContextAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        DbSet<T> Set<T>()
            where T: class;

        DatabaseFacade Database { get; }

        ChangeTracker ChangeTracker { get; }

        EntityEntry Add([NotNull] object entity);

        EntityEntry<TEntity> Add<TEntity>([NotNull] TEntity entity) where TEntity : class;

        ValueTask<EntityEntry> AddAsync([NotNull] object entity, CancellationToken cancellationToken = default);

        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>([NotNull] TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;

        void AddRange([NotNull] IEnumerable<object> entities);

        void AddRange([NotNull] params object[] entities);

        Task AddRangeAsync([NotNull] params object[] entities);

        Task AddRangeAsync([NotNull] IEnumerable<object> entities, CancellationToken cancellationToken = default);

        void AttachRange([NotNull] IEnumerable<object> entities);

        void AttachRange([NotNull] params object[] entities);

        EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;

        EntityEntry Entry([NotNull] object entity);

        object Find([NotNull] Type entityType, [NotNull] params object[] keyValues);

        TEntity Find<TEntity>([NotNull] params object[] keyValues) where TEntity : class;

        ValueTask<object> FindAsync([NotNull] Type entityType, [NotNull] object[] keyValues, CancellationToken cancellationToken);

        ValueTask<TEntity> FindAsync<TEntity>([NotNull] object[] keyValues, CancellationToken cancellationToken) where TEntity : class;

        ValueTask<TEntity> FindAsync<TEntity>([NotNull] params object[] keyValues) where TEntity : class;

        ValueTask<object> FindAsync([NotNull] Type entityType, [NotNull] params object[] keyValues);

        EntityEntry<TEntity> Remove<TEntity>([NotNull] TEntity entity) where TEntity : class;

        EntityEntry Remove([NotNull] object entity);

        void RemoveRange([NotNull] IEnumerable<object> entities);

        void RemoveRange([NotNull] params object[] entities);

        EntityEntry<TEntity> Update<TEntity>([NotNull] TEntity entity) where TEntity : class;

        EntityEntry Update([NotNull] object entity);

        void UpdateRange([NotNull] params object[] entities);

        void UpdateRange([NotNull] IEnumerable<object> entities);
    }
}
