using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity;

public class UserEntityUpdatedEventHandler : ILocalEventHandler<EntityUpdatedEventData<IdentityUser>>, ITransientDependency
{
    public ILogger<UserEntityUpdatedEventHandler> Logger { get; set; }

    private readonly IdentityDynamicClaimsPrincipalContributorCache _cache;

    public UserEntityUpdatedEventHandler(IdentityDynamicClaimsPrincipalContributorCache cache)
    {
        _cache = cache;
        Logger = NullLogger<UserEntityUpdatedEventHandler>.Instance;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityUpdatedEventData<IdentityUser> eventData)
    {
        var userId = eventData.Entity.Id;
        Logger.LogDebug($"Clearing dynamic claims cache for user: {userId}");
        await _cache.ClearAsync(userId, eventData.Entity.TenantId);
    }
}
