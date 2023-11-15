using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity;

public class UserEntityUpdatedEventHandler :
    ILocalEventHandler<EntityUpdatedEventData<IdentityUser>>,
    ILocalEventHandler<EntityDeletedEventData<IdentityUser>>,
    ITransientDependency
{
    private readonly IdentityDynamicClaimsPrincipalContributorCache _cache;

    public UserEntityUpdatedEventHandler(IdentityDynamicClaimsPrincipalContributorCache cache)
    {
        _cache = cache;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityUpdatedEventData<IdentityUser> eventData)
    {
        await ClearAsync(eventData.Entity.Id, eventData.Entity.TenantId);
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<IdentityUser> eventData)
    {
        await ClearAsync(eventData.Entity.Id, eventData.Entity.TenantId);
    }

    protected virtual async Task ClearAsync(Guid userId, Guid? tenantId)
    {
        await _cache.ClearAsync(userId, tenantId);
    }
}
