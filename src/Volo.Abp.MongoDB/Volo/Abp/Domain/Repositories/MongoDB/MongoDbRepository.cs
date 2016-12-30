using System.Linq;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public class MongoDbRepository<TMongoDbContext, TEntity> : MongoDbRepository<TMongoDbContext, TEntity, string>, IMongoDbRepository<TEntity>
        where TMongoDbContext : AbpMongoDbContext
        where TEntity : class, IEntity<string>
    {
        public MongoDbRepository(IMongoDatabaseProvider<TMongoDbContext> databaseProvider)
            : base(databaseProvider)
        {
        }
    }

    //TODO: MongoDb.Driver fully supports async, implement all of them!

    public class MongoDbRepository<TMongoDbContext, TEntity, TPrimaryKey> : QueryableRepositoryBase<TEntity, TPrimaryKey>, IMongoDbRepository<TEntity, TPrimaryKey> 
        where TMongoDbContext : AbpMongoDbContext
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public virtual string CollectionName
        {
            get { return typeof(TEntity).Name; } //TODO: a better naming, or a way of easily overriding it?
        }

        public virtual IMongoCollection<TEntity> Collection => Database.GetCollection<TEntity>(CollectionName);

        public virtual IMongoDatabase Database => DatabaseProvider.GetDatabase();

        protected IMongoDatabaseProvider<TMongoDbContext> DatabaseProvider { get; }

        public MongoDbRepository(IMongoDatabaseProvider<TMongoDbContext> databaseProvider)
        {
            DatabaseProvider = databaseProvider;
        }

        public override TEntity Insert(TEntity entity, bool autoSave = false)
        {
            //TODO: Mongo has InsertMany & UpdateMany methods. Does them transactional? If so, we may consider to add them to IRepository!

            Collection.InsertOne(entity);
            return entity;
        }

        public override TEntity Update(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            Collection.ReplaceOne(filter, entity);
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            Collection.DeleteOne(filter);
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return Collection.AsQueryable();
        }
    }
}