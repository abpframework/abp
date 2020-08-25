using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB
{
    public abstract class AbpMongoDbContext : IAbpMongoDbContext, ITransientDependency
    {
        public IMongoModelSource ModelSource { get; set; }

        public IMongoDatabase Database { get; private set; }

        public IClientSessionHandle SessionHandle { get; private set; }

        protected internal virtual void CreateModel(IMongoModelBuilder modelBuilder)
        {

        }

        public virtual void InitializeDatabase(IMongoDatabase database, IClientSessionHandle sessionHandle)
        {
            Database = database;
            SessionHandle = sessionHandle;
        }

        public virtual IMongoCollection<T> Collection<T>()
        {
            CreateCollectionIfNotExists<T>();

            return Database.GetCollection<T>(GetCollectionName<T>());
        }

        protected virtual string GetCollectionName<T>()
        {
            return GetEntityModel<T>().CollectionName;
        }

        protected virtual IMongoEntityModel GetEntityModel<TEntity>()
        {
            var model = ModelSource.GetModel(this).Entities.GetOrDefault(typeof(TEntity));

            if (model == null)
            {
                throw new AbpException("Could not find a model for given entity type: " + typeof(TEntity).AssemblyQualifiedName);
            }

            return model;
        }

        protected virtual void CreateCollectionIfNotExists<T>()
        {
            var collectionName = GetCollectionName<T>();

            if (!CollectionExists(collectionName))
            {
                Database.CreateCollection(collectionName);
            }
        }

        protected virtual bool CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            return Database.ListCollectionNames(options).Any();
        }
    }
}
