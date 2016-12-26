using System.Linq;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public class MongoDbRepository<TEntity> : MongoDbRepository<TEntity, string>, IMongoDbRepository<TEntity>
        where TEntity : class, IEntity<string>
    {
        public MongoDbRepository(IMongoDatabaseProvider databaseProvider)
            : base(databaseProvider)
        {
        }
    }

    //TODO: MongoDb.Driver fully supports async, implement all of them!

    public class MongoDbRepository<TEntity, TPrimaryKey> : QueryableRepositoryBase<TEntity, TPrimaryKey>, IMongoDbRepository<TEntity, TPrimaryKey> 
        where TEntity : class, IEntity<TPrimaryKey>
    {
        //TODO: Define a MongoDbContext to relate to a connection string for modularity!

        public virtual IMongoCollection<TEntity> Collection => Database.GetCollection<TEntity>("");

        public virtual IMongoDatabase Database => _databaseProvider.GetDatabase();

        private readonly IMongoDatabaseProvider _databaseProvider;

        public MongoDbRepository(IMongoDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public override TEntity Insert(TEntity entity, bool autoSave = false)
        {
            //TODO: What about assigning PK? Does mongodb handle it? Test!
            //TODO: Mongo has InsertMany & UpdateMany methods. Does them transactional? If so, we may consider to add them to IRepository!

            Collection.InsertOne(entity);
            return entity;
        }

        public override TEntity Update(TEntity entity)
        {
            //TODO: How to update? TEST!

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            Collection.UpdateOne(filter, new ObjectUpdateDefinition<TEntity>(entity));
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            //TODO: How to delete? TEST!

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            Collection.DeleteOne(filter);
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return Collection.AsQueryable();
        }
    }
}