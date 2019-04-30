using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Domain.Entities.Events.Distributed
{
    public class EntityToEtoMapper : IEntityToEtoMapper, ITransientDependency
    {
        protected IObjectMapper ObjectMapper { get; }
        protected DistributedEventBusOptions Options { get; }

        public EntityToEtoMapper(
            IOptions<DistributedEventBusOptions> options,
            IObjectMapper objectMapper)
        {
            ObjectMapper = objectMapper;
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
            var etoType = Options.EtoMappings.GetOrDefault(entityType);
            if (etoType == null)
            {
                var keys = entity.GetKeys().JoinAsString(",");
                return new EntityEto(entityType.FullName, keys);
            }

            //TODO: Also add KeysAsString property to resulting json for compatibility with the EntityEto!
            return ObjectMapper.Map(entityType, etoType, entityObj);
        }
    }
}