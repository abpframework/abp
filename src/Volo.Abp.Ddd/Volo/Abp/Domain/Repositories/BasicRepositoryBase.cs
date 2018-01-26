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

        public abstract TEntity Update(TEntity entity);

        public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Update(entity));
        }

        public abstract void Delete(TEntity entity);

        public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
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
        public virtual TEntity Get(TKey id)
        {
            var entity = Find(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Get(id));
        }

        public abstract TEntity Find(TKey id);

        public virtual Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Find(id));
        }

        public virtual void Delete(TKey id)
        {
            var entity = Find(id);
            if (entity == null)
            {
                return;
            }

            Delete(entity);
        }

        public virtual Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            Delete(id);
            return Task.CompletedTask;
        }
    }
}
