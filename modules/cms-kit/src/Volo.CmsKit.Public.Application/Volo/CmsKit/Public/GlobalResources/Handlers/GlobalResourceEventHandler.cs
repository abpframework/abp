using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectMapping;
using Volo.CmsKit.GlobalResources;
using Volo.CmsKit.Public.GlobalResources;

namespace Volo.CmsKit.Public.GlobalResources.Handlers;

public class GlobalResourceEventHandler: 
    ILocalEventHandler<EntityUpdatedEventData<GlobalResource>>,
    ITransientDependency
{
    public IObjectMapper ObjectMapper { get; }
    private readonly IDistributedCache<GlobalResourceDto> _resourceCache;

    public GlobalResourceEventHandler(
        IDistributedCache<GlobalResourceDto> resourceCache,
        IObjectMapper objectMapper)
    {
        ObjectMapper = objectMapper;
        _resourceCache = resourceCache;
    }
    
    public async Task HandleEventAsync(EntityUpdatedEventData<GlobalResource> eventData)
    {
        await _resourceCache.SetAsync(
            eventData.Entity.Name, 
            ObjectMapper.Map<GlobalResource, GlobalResourceDto>(eventData.Entity));
    }
}