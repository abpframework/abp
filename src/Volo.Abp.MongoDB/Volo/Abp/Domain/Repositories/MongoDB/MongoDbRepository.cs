using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public class MongoDbRepository<TMongoDbContext, TEntity> : RepositoryBase<TEntity>, IMongoDbRepository<TEntity>
        where TMongoDbContext : IAbpMongoDbContext
        where TEntity : class, IEntity
    {
        public virtual IMongoCollection<TEntity> Collection => DbContext.Collection<TEntity>();

        public virtual IMongoDatabase Database => DbContext.Database;

        public virtual TMongoDbContext DbContext => DbContextProvider.GetDbContext();

        protected IMongoDbContextProvider<TMongoDbContext> DbContextProvider { get; }

        public MongoDbRepository(IMongoDbContextProvider<TMongoDbContext> dbContextProvider)
        {
            DbContextProvider = dbContextProvider;
        }

        public override TEntity Insert(TEntity entity, bool autoSave = false)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public override TEntity Update(TEntity entity, bool autoSave = false)
        {
            Collection.ReplaceOne(CreateEntityFilter(entity), entity);
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Collection.ReplaceOneAsync(CreateEntityFilter(entity), entity, cancellationToken: cancellationToken);
            return entity;
        }

        public override void Delete(TEntity entity, bool autoSave = false)
        {
            Collection.DeleteOne(CreateEntityFilter(entity));
        }

        public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Collection.DeleteOneAsync(CreateEntityFilter(entity), cancellationToken);
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate, bool autoSave = false)
        {
            Collection.DeleteMany(Builders<TEntity>.Filter.Where(predicate));
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Collection.DeleteManyAsync(Builders<TEntity>.Filter.Where(predicate), cancellationToken);
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return ApplyDataFilters(Collection.AsQueryable());
        }

        protected virtual FilterDefinition<TEntity> CreateEntityFilter(TEntity entity)
        {
            throw new NotImplementedException("CreateEntityFilter is not implemented for MongoDb by default. It should be overrided and implemented by deriving classes!");
        }
    }

    public class MongoDbRepository<TMongoDbContext, TEntity, TKey> : MongoDbRepository<TMongoDbContext, TEntity>, IMongoDbRepository<TEntity, TKey>
        where TMongoDbContext : IAbpMongoDbContext
        where TEntity : class, IEntity<TKey>
    {
        public MongoDbRepository(IMongoDbContextProvider<TMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual TEntity Get(TKey id)
        {
            var entity = Find(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(CreateEntityFilter(id, true)).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual TEntity Find(TKey id)
        {
            return Collection.Find(CreateEntityFilter(id, true)).FirstOrDefault();
        }

        public virtual void Delete(TKey id, bool autoSave = false)
        {
            Collection.DeleteOne(CreateEntityFilter(id));
        }

        public virtual Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Collection.DeleteOneAsync(CreateEntityFilter(id), cancellationToken);
        }

        protected override FilterDefinition<TEntity> CreateEntityFilter(TEntity entity)
        {
            return Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
        }

        protected virtual FilterDefinition<TEntity> CreateEntityFilter(TKey id, bool applyFilters = false)
        {
            var filters = new List<FilterDefinition<TEntity>>
            {
                Builders<TEntity>.Filter.Eq(e => e.Id, id)
            };

            if (applyFilters)
            {
                AddGlobalFilters(filters);
            }

            return Builders<TEntity>.Filter.And(filters);
        }

        protected virtual void AddGlobalFilters(List<FilterDefinition<TEntity>> filters)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) && DataFilter.IsEnabled<ISoftDelete>())
            {
                filters.Add(Builders<TEntity>.Filter.Eq(e => ((ISoftDelete) e).IsDeleted, false));
            }

            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                var tenantId = CurrentTenant.Id;
                filters.Add(Builders<TEntity>.Filter.Eq(e => ((IMultiTenant) e).TenantId, tenantId));
            }
        }
    }
}