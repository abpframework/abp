using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public class MongoDbRepositoryRegistrar : RepositoryRegistrarBase<MongoDbContextRegistrationOptions>
    {
        public MongoDbRepositoryRegistrar(MongoDbContextRegistrationOptions options)
            : base(options)
        {
        }

        public override IEnumerable<Type> GetEntityTypes(Type dbContextType)
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