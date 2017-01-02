using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MongoDB;

namespace Volo.Abp.MongoDB.DependencyInjection
{
    public class MongoDbRepositoryRegistrar : RepositoryRegistrarBase<MongoDbContextRegistrationOptions>
    {
        public MongoDbRepositoryRegistrar(MongoDbContextRegistrationOptions options)
            : base(options)
        {
        }

        protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
        {
            var mongoDbContext = (AbpMongoDbContext)Activator.CreateInstance(dbContextType);
            return mongoDbContext.GetEntityCollectionTypes();
        }

        protected override Type GetRepositoryTypeForDefaultPk(Type dbContextType, Type entityType)
        {
            return typeof(MongoDbRepository<,>).MakeGenericType(dbContextType, entityType);
        }

        protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
        {
            return typeof(MongoDbRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
        }
    }
}