using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories
{
    public abstract class QueryableRepositoryBase<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>, IQueryableRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public abstract IQueryable<TEntity> GetAll();

        public abstract List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public abstract TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public abstract void Delete(Expression<Func<TEntity, bool>> predicate);

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(predicate);
            return Task.CompletedTask;
        }

        public abstract int Count(Expression<Func<TEntity, bool>> predicate);

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }
    }
}