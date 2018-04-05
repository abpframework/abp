using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Threading;

namespace Volo.Abp.Domain.Repositories
{
    public abstract class BasicRepositoryBase<TEntity> : IBasicRepository<TEntity>
        where TEntity : class, IEntity
    {
        public ICancellationTokenProvider CancellationTokenProvider { get; set; }

        protected BasicRepositoryBase()
        {
            CancellationTokenProvider = NullCancellationTokenProvider.Instance;
        }

        public abstract TEntity Insert(TEntity entity, bool autoSave = false);

        public virtual Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Insert(entity, autoSave));
        }

        public abstract TEntity Update(TEntity entity, bool autoSave = false);

        public virtual Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Update(entity));
        }

        public abstract void Delete(TEntity entity, bool autoSave = false);

        public virtual Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Delete(entity);
            return Task.CompletedTask;
        }

        protected virtual CancellationToken GetCancellationToken(CancellationToken prefferedValue = default)
        {
            return CancellationTokenProvider.FallbackToProvider(prefferedValue);
        }
    }

    public abstract class BasicRepositoryBase<TEntity, TKey> : BasicRepositoryBase<TEntity>, IBasicRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public virtual TEntity Get(TKey id, bool includeDetails = true)
        {
            var entity = Find(id, includeDetails);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Get(id, includeDetails));
        }

        public abstract TEntity Find(TKey id, bool includeDetails = true);

        public virtual Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Find(id, includeDetails));
        }

        public virtual void Delete(TKey id, bool autoSave = false)
        {
            var entity = Find(id);
            if (entity == null)
            {
                return;
            }

            Delete(entity);
        }

        public virtual Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Delete(id);
            return Task.CompletedTask;
        }
    }
}
