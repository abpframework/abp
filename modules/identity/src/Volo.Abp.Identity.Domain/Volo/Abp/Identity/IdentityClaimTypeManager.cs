using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.Abp.Identity;

public class IdentityClaimTypeManager : DomainService
{
    protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }

    public IdentityClaimTypeManager(IIdentityClaimTypeRepository identityClaimTypeRepository)
    {
        IdentityClaimTypeRepository = identityClaimTypeRepository;
    }

    public virtual async Task<IdentityClaimType> CreateAsync(IdentityClaimType claimType)
    {
        if (await IdentityClaimTypeRepository.AnyAsync(claimType.Name))
        {
            throw new AbpException($"Name Exist: {claimType.Name}");
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
}
