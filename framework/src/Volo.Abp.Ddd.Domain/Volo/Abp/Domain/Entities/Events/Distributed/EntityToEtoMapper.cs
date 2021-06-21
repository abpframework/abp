using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Domain.Entities.Events.Distributed
{
    public class EntityToEtoMapper : IEntityToEtoMapper, ITransientDependency
    {
        protected IServiceScopeFactory HybridServiceScopeFactory { get; }

        protected AbpDistributedEntityEventOptions Options { get; }

        public EntityToEtoMapper(
            IOptions<AbpDistributedEntityEventOptions> options,
            IServiceScopeFactory hybridServiceScopeFactory)
        {
            HybridServiceScopeFactory = hybridServiceScopeFactory;
            Options = options.Value;
        }

        public object Map(object entityObj)
        {
            Check.NotNull(entityObj, nameof(entityObj));

            var entity = entityObj as IEntity;
            if (entity == null)
            {
                return null;
            }

            var entityType = ProxyHelper.UnProxy(entity).GetType();
            var etoMappingItem = Options.EtoMappings.GetOrDefault(entityType);
            if (etoMappingItem == null)
            {
                var keys = entity.GetKeys().JoinAsString(",");
                return new EntityEto(entityType.FullName, keys);
            }

            using (var scope = HybridServiceScopeFactory.CreateScope())
            {
                var objectMapperType = etoMappingItem.ObjectMappingContextType == null
                    ? typeof(IObjectMapper)
                    : typeof(IObjectMapper<>).MakeGenericType(etoMappingItem.ObjectMappingContextType);

                var objectMapper = (IObjectMapper) scope.ServiceProvider.GetRequiredService(objectMapperType);

                return objectMapper.Map(entityType, etoMappingItem.EtoType, entityObj);
            }
        }
    }
}
