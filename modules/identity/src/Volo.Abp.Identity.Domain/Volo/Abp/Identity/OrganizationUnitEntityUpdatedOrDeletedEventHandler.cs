using System;
using System.Collections.Generic;
using System.Linq;
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

public class OrganizationUnitEntityUpdatedOrDeletedEventHandler :
    ILocalEventHandler<EntityUpdatedEventData<OrganizationUnit>>,
    ILocalEventHandler<EntityDeletedEventData<OrganizationUnit>>,
    ILocalEventHandler<EntityCreatedEventData<OrganizationUnitRole>>,
    ITransientDependency
{
    public ILogger<OrganizationUnitEntityUpdatedOrDeletedEventHandler> Logger { get; set; }

    private readonly IOrganizationUnitRepository _organizationUnitRepository;
    private readonly IDistributedCache<AbpDynamicClaimCacheItem> _cache;

    public OrganizationUnitEntityUpdatedOrDeletedEventHandler(
        IOrganizationUnitRepository organizationUnitRepository,
        IDistributedCache<AbpDynamicClaimCacheItem> cache)
    {
        Logger = NullLogger<OrganizationUnitEntityUpdatedOrDeletedEventHandler>.Instance;

        _organizationUnitRepository = organizationUnitRepository;
        _cache = cache;
    }

    public virtual async Task HandleEventAsync(EntityUpdatedEventData<OrganizationUnit> eventData)
    {
        var users = await _organizationUnitRepository.GetMemberIdsAsync(eventData.Entity.Id);
        await ClearAsync(eventData.Entity.Id, users, eventData.Entity.TenantId);
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<OrganizationUnit> eventData)
    {
        var users = await _organizationUnitRepository.GetMemberIdsAsync(eventData.Entity.Id);
        await ClearAsync(eventData.Entity.Id, users, eventData.Entity.TenantId);
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityCreatedEventData<OrganizationUnitRole> eventData)
    {
        var users = await _organizationUnitRepository.GetMemberIdsAsync(eventData.Entity.OrganizationUnitId);
        await ClearAsync(eventData.Entity.OrganizationUnitId, users, eventData.Entity.TenantId);
    }

    protected virtual async Task ClearAsync(Guid organizationId, IEnumerable<Guid> userIds, Guid? tenantId)
    {
        Logger.LogDebug($"Remove dynamic claims cache for users of organization: {organizationId}");
        await _cache.RemoveManyAsync(userIds.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId)));
    }
}
