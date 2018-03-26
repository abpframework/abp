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
            //TODO: Instead of getting from Options.OriginalDbContextType, we may consider to add entities as properties to the dbcontext, just like EF Core!
            var mongoDbContext = (IAbpMongoDbContext)Activator.CreateInstance(Options.OriginalDbContextType);
            return mongoDbContext.GetMappings().Select(m => m.EntityType);
        }

        protected override Type GetRepositoryType(Type dbContextType, Type entityType)
        {
            return typeof(MongoDbRepository<,,>).MakeGenericType(dbContextType, entityType);
        }

        protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
        {
            return typeof(MongoDbRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
        }
    }
}