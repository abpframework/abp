using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public class MongoDbRepository<TMongoDbContext, TEntity> : MongoDbRepository<TMongoDbContext, TEntity, Guid>, IMongoDbRepository<TEntity>
        where TMongoDbContext : AbpMongoDbContext
        where TEntity : class, IEntity<Guid>
    {
        public MongoDbRepository(IMongoDatabaseProvider<TMongoDbContext> databaseProvider)
            : base(databaseProvider)
        {
        }
    }

    public class MongoDbRepository<TMongoDbContext, TEntity, TPrimaryKey> : QueryableRepositoryBase<TEntity, TPrimaryKey>, IMongoDbRepository<TEntity, TPrimaryKey> 
        where TMongoDbContext : AbpMongoDbContext
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public virtual string CollectionName => DatabaseProvider.DbContext.GetCollectionName<TEntity>();

        public virtual IMongoCollection<TEntity> Collection => Database.GetCollection<TEntity>(CollectionName);

        public virtual IMongoDatabase Database => DatabaseProvider.GetDatabase();

        protected IMongoDatabaseProvider<TMongoDbContext> DatabaseProvider { get; }

        public MongoDbRepository(IMongoDatabaseProvider<TMongoDbContext> databaseProvider)
        {
            DatabaseProvider = databaseProvider;
        }

        //TODO: Override other methods?

        public override async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await (await Collection.FindAsync(Builders<TEntity>.Filter.Empty, cancellationToken: cancellationToken)).ToListAsync(cancellationToken);
        }

        public override async Task<TEntity> GetAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
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
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            Collection.ReplaceOne(filter, entity);
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            await Collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            Delete(entity.Id);
        }

        public override void Delete(TPrimaryKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            Collection.DeleteOne(filter);
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var filter = Builders<TEntity>.Filter.Where(predicate);
            Collection.DeleteOne(filter);
        }

        public override Task DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            return Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        }

        public override Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return DeleteAsync(entity.Id, cancellationToken);
        }

        public override Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.Where(predicate);
            return Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return Collection.AsQueryable();
        }

        public override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return Collection.CountAsync(Builders<TEntity>.Filter.Empty, cancellationToken: cancellationToken);
        }
    }
}