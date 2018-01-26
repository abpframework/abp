using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public class MongoDbRepository<TMongoDbContext, TEntity> : RepositoryBase<TEntity>, IMongoDbRepository<TEntity>
        where TMongoDbContext : AbpMongoDbContext
        where TEntity : class, IEntity
    {
        public virtual string CollectionName => DatabaseProvider.DbContext.GetCollectionName<TEntity>();

        public virtual IMongoCollection<TEntity> Collection => Database.GetCollection<TEntity>(CollectionName);

        public virtual IMongoDatabase Database => DatabaseProvider.GetDatabase();

        protected IMongoDatabaseProvider<TMongoDbContext> DatabaseProvider { get; }

        public MongoDbRepository(IMongoDatabaseProvider<TMongoDbContext> databaseProvider)
        {
            DatabaseProvider = databaseProvider;
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

        public override TEntity Update(TEntity entity)
        {
            Collection.ReplaceOne(CreateEntityFilter(entity), entity);
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Collection.ReplaceOneAsync(CreateEntityFilter(entity), entity, cancellationToken: cancellationToken);
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            Collection.DeleteOne(CreateEntityFilter(entity));
        }

        public override async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Collection.DeleteOneAsync(CreateEntityFilter(entity), cancellationToken);
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            Collection.DeleteMany(Builders<TEntity>.Filter.Where(predicate));
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            await Collection.DeleteManyAsync(Builders<TEntity>.Filter.Where(predicate), cancellationToken);
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return Collection.AsQueryable();
        }

        protected virtual FilterDefinition<TEntity> CreateEntityFilter(TEntity entity)
        {
            throw new NotImplementedException("CreateEntityFilter is not implemented for MongoDb by default. It should be overrided and implemented by deriving classes!");
        }
    }

    public class MongoDbRepository<TMongoDbContext, TEntity, TKey> : MongoDbRepository<TMongoDbContext, TEntity>, IMongoDbRepository<TEntity, TKey>
        where TMongoDbContext : AbpMongoDbContext
        where TEntity : class, IEntity<TKey>
    {
        public MongoDbRepository(IMongoDatabaseProvider<TMongoDbContext> databaseProvider)
            : base(databaseProvider)
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

        public virtual void Delete(TKey id)
        {
            Collection.DeleteOne(CreateEntityFilter(id));
        }

        public virtual Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Collection.DeleteOneAsync(CreateEntityFilter(id), cancellationToken);
        }

        public virtual async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(CreateEntityFilter(id)).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual TEntity Find(TKey id)
        {
            return Collection.Find(CreateEntityFilter(id)).FirstOrDefault();
        }

        protected override FilterDefinition<TEntity> CreateEntityFilter(TEntity entity)
        {
            return Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
        }

        private static FilterDefinition<TEntity> CreateEntityFilter(TKey id)
        {
            return Builders<TEntity>.Filter.Eq(e => e.Id, id);
        }
    }
}