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
    
    public virtual async Task<List<IdentityUserDelegation>> GetActiveDelegationsAsync(Guid sourceUserId, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.GetActiveDelegationsAsync(sourceUserId, cancellationToken: cancellationToken);
    }

    public virtual async Task<IdentityUserDelegation> FindActiveDelegationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.FindActiveDelegationByIdAsync(id, cancellationToken: cancellationToken);
    }

    public virtual async Task DelegateNewUserAsync(Guid sourceUserId, IdentityUser targetUser, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default)
    {
        if (sourceUserId == targetUser.Id)
        {
            throw new BusinessException(IdentityErrorCodes.YouCannotDelegateYourself);
        }
        
        await IdentityUserDelegationRepository.InsertAsync(
            new IdentityUserDelegation(
                GuidGenerator.Create(),
                sourceUserId,
                targetUser.Id,
                startTime,
                endTime
            ),
            cancellationToken: cancellationToken
        );
    }

    public virtual async Task DeleteDelegationAsync(Guid id, Guid sourceUserId, CancellationToken cancellationToken = default)
    {
        var delegation = await IdentityUserDelegationRepository.FindAsync(id, cancellationToken: cancellationToken);

        if (delegation != null && delegation.SourceUserId == sourceUserId)
        {
            await IdentityUserDelegationRepository.DeleteAsync(delegation, cancellationToken: cancellationToken);
        }
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
