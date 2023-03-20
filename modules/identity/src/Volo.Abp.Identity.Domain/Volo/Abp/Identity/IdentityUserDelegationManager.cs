using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.Abp.Identity;

public class IdentityUserDelegationManager : DomainService
{
    protected IIdentityUserDelegationRepository IdentityUserDelegationRepository { get; }

    public IdentityUserDelegationManager(IIdentityUserDelegationRepository identityUserDelegationRepository)
    {
        IdentityUserDelegationRepository = identityUserDelegationRepository;
    }

    public virtual async Task<List<IdentityUserDelegation>> GetListAsync(Guid? sourceUserId = null, Guid? targetUserId = null, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.GetListAsync(sourceUserId, targetUserId, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteDelegationAsync(Guid id, Guid sourceUserId, CancellationToken cancellationToken = default)
    {
        var delegation = await IdentityUserDelegationRepository.FindAsync(id, cancellationToken: cancellationToken);

        if (delegation != null && delegation.SourceUserId == sourceUserId)
        {
            await IdentityUserDelegationRepository.DeleteAsync(delegation, cancellationToken: cancellationToken);
        }
    }

    public virtual async Task<bool> IsDelegatedAsync(Guid sourceUserId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.FindAsync(sourceUserId, targetUserId, cancellationToken: cancellationToken) != null;
    }

    public virtual Task<bool> IsExpiredAsync(IdentityUserDelegation userDelegation)
    {
        return Task.FromResult(userDelegation.EndTime <= Clock.Now);
    }

    public virtual async Task<bool> IsValidAsync(IdentityUserDelegation userDelegation)
    {
        return userDelegation.StartTime <= Clock.Now && !await IsExpiredAsync(userDelegation);
    }
}
