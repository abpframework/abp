using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity;

//Sessions, user delegations and link users have no navigation from IdentityUser,
//so clearing the user's collections doesn't cover them.
public class UserDeletedEventHandler :
    ILocalEventHandler<EntityDeletedEventData<IdentityUser>>,
    ITransientDependency
{
    protected IIdentitySessionRepository IdentitySessionRepository { get; }
    protected IIdentityUserDelegationRepository IdentityUserDelegationRepository { get; }
    protected IIdentityLinkUserRepository IdentityLinkUserRepository { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public UserDeletedEventHandler(
        IIdentitySessionRepository identitySessionRepository,
        IIdentityUserDelegationRepository identityUserDelegationRepository,
        IIdentityLinkUserRepository identityLinkUserRepository,
        ICurrentTenant currentTenant)
    {
        IdentitySessionRepository = identitySessionRepository;
        IdentityUserDelegationRepository = identityUserDelegationRepository;
        IdentityLinkUserRepository = identityLinkUserRepository;
        CurrentTenant = currentTenant;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<IdentityUser> eventData)
    {
        var user = eventData.Entity;

        await IdentitySessionRepository.DeleteAllAsync(user.Id);

        var delegations = await IdentityUserDelegationRepository.GetListAsync(sourceUserId: user.Id, targetUserId: null);
        delegations.AddRange(await IdentityUserDelegationRepository.GetListAsync(sourceUserId: null, targetUserId: user.Id));
        //A delegation of the user to itself is returned by both queries.
        await IdentityUserDelegationRepository.DeleteManyAsync(delegations.DistinctBy(x => x.Id).ToList());

        //Link users are stored in the host database.
        using (CurrentTenant.Change(null))
        {
            await IdentityLinkUserRepository.DeleteAsync(new IdentityLinkUserInfo(user.Id, user.TenantId));
        }
    }
}
