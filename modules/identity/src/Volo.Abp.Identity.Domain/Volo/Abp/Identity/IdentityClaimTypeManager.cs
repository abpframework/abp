using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.Abp.Identity;

public class IdentityClaimTypeManager : DomainService
{
    protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }
    protected IIdentityUserRepository IdentityUserRepository { get; }

    public IdentityClaimTypeManager(IIdentityClaimTypeRepository identityClaimTypeRepository, IIdentityUserRepository identityUserRepository)
    {
        IdentityClaimTypeRepository = identityClaimTypeRepository;
        IdentityUserRepository = identityUserRepository;
    }

    public virtual async Task<IdentityClaimType> CreateAsync(IdentityClaimType claimType)
    {
        if (await IdentityClaimTypeRepository.AnyAsync(claimType.Name))
        {
            throw new BusinessException(IdentityErrorCodes.ClaimNameExist).WithData("0", claimType.Name);
        }

        return await IdentityClaimTypeRepository.InsertAsync(claimType);
    }

    public virtual async Task<IdentityClaimType> UpdateAsync(IdentityClaimType claimType)
    {
        if (await IdentityClaimTypeRepository.AnyAsync(claimType.Name, claimType.Id))
        {
            throw new AbpException($"Name Exist: {claimType.Name}");
        }

        if (claimType.IsStatic)
        {
            throw new AbpException($"Can not update a static ClaimType.");
        }


        return await IdentityClaimTypeRepository.UpdateAsync(claimType);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var claimType = await IdentityClaimTypeRepository.GetAsync(id);
        if (claimType.IsStatic)
        {
            throw new AbpException($"Can not delete a static ClaimType.");
        }

        //Remove claim of this type from all users
        await IdentityUserRepository.RemoveClaimFromAllUsersAsync(claimType.Name);
        await IdentityClaimTypeRepository.DeleteAsync(id);
    }
}
