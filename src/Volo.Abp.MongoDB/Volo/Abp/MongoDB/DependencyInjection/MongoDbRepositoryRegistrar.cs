using System;
using System.Collections.Generic;
using System.Linq;
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
            return mongoDbContext.GetMappings().Select(m => m.EntityType);
        }

        protected override Type GetRepositoryType(Type dbContextType, Type entityType)
        {
            throw new NotImplementedException();
        }

        protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
        {
            return typeof(MongoDbRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
        }
    }
}