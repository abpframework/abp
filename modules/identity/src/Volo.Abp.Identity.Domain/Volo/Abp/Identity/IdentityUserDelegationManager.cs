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
    
    public virtual async Task<List<IdentityUserDelegation>> GetActiveDelegationsAsync(Guid targetUseId, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.GetActiveDelegationsAsync(targetUseId, cancellationToken: cancellationToken);
    }

    public virtual async Task<IdentityUserDelegation> FindActiveDelegationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.FindActiveDelegationByIdAsync(id, cancellationToken: cancellationToken);
    }

    public virtual async Task DelegateNewUserAsync(Guid sourceUserId, Guid targetUserId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default)
    {
        if (sourceUserId == targetUserId)
        {
            throw new BusinessException(IdentityErrorCodes.YouCannotDelegateYourself);
        }
        
        await IdentityUserDelegationRepository.InsertAsync(
            new IdentityUserDelegation(
                GuidGenerator.Create(),
                sourceUserId,
                targetUserId,
                startTime,
                endTime,
                CurrentTenant.Id
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
}
