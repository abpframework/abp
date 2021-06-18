using System.Collections.Generic;
using MongoDB.Driver;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB
{
    public abstract class AbpMongoDbContext : IAbpMongoDbContext, ITransientDependency
    {
        public IMongoModelSource ModelSource { get; set; }

        public IMongoClient Client { get; private set; }

        public IMongoDatabase Database { get; private set; }

        public IClientSessionHandle SessionHandle { get; private set; }

        protected internal virtual void CreateModel(IMongoModelBuilder modelBuilder)
        {

        }

        public virtual void InitializeDatabase(IMongoDatabase database, IMongoClient client, IClientSessionHandle sessionHandle)
        {
            Database = database;
            Client = client;
            SessionHandle = sessionHandle;
        }

        public virtual IMongoCollection<T> Collection<T>()
        {
            return Database.GetCollection<T>(GetCollectionName<T>());
        }

        public virtual void InitializeCollections(IMongoDatabase database)
        {
            Database = database;
            ModelSource.GetModel(this);
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
    }
}
