using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MemoryDb;

namespace Volo.Abp.MemoryDb.DependencyInjection
{
    public class MemoryDbRepositoryRegistrar : RepositoryRegistrarBase<AbpMemoryDbContextRegistrationOptions>
    {
        public MemoryDbRepositoryRegistrar(AbpMemoryDbContextRegistrationOptions options)
            : base(options)
        {
        }

        protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
        {
            var memoryDbContext = (MemoryDbContext)Activator.CreateInstance(dbContextType);
            return memoryDbContext.GetEntityTypes();
        }

        protected override Type GetRepositoryType(Type dbContextType, Type entityType)
        {
            return typeof(MemoryDbRepository<,>).MakeGenericType(dbContextType, entityType);
        }

        protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
        {
            return typeof(MemoryDbRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
        }
    }
}