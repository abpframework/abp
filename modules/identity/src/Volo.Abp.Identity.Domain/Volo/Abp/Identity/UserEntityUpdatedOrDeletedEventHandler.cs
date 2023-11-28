using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity;

public class UserEntityUpdatedOrDeletedEventHandler :
    ILocalEventHandler<EntityUpdatedEventData<IdentityUser>>,
    ILocalEventHandler<EntityDeletedEventData<IdentityUser>>,
    ITransientDependency
{
    public ILogger<UserEntityUpdatedOrDeletedEventHandler> Logger { get; set; }

    private readonly IDistributedCache<AbpDynamicClaimCacheItem> _dynamicClaimCache;

    public UserEntityUpdatedOrDeletedEventHandler(IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache)
    {
        Logger = NullLogger<UserEntityUpdatedOrDeletedEventHandler>.Instance;

        _dynamicClaimCache = dynamicClaimCache;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityUpdatedEventData<IdentityUser> eventData)
    {
        await RemoveDynamicClaimCacheAsync(eventData.Entity.Id, eventData.Entity.TenantId);
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<IdentityUser> eventData)
    {
        await RemoveDynamicClaimCacheAsync(eventData.Entity.Id, eventData.Entity.TenantId);
    }

    protected virtual async Task RemoveDynamicClaimCacheAsync(Guid userId, Guid? tenantId)
    {
        Logger.LogDebug($"Remove dynamic claims cache for user: {userId}");
        await _dynamicClaimCache.RemoveAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId));
    }
}
