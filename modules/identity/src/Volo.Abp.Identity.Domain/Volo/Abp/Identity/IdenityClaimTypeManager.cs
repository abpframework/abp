using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.Abp.Identity
{
    public class IdenityClaimTypeManager : DomainService
    {
        private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;

        public IdenityClaimTypeManager(IIdentityClaimTypeRepository identityClaimTypeRepository)
        {
            _identityClaimTypeRepository = identityClaimTypeRepository;
        }

        public virtual async Task<IdentityClaimType> CreateAsync(IdentityClaimType claimType)
        {
            if (await _identityClaimTypeRepository.AnyAsync(claimType.Name))
            {
                throw new AbpException($"Name Exist: {claimType.Name}");
            }

            return await _identityClaimTypeRepository.InsertAsync(claimType);
        }

        public virtual async Task<IdentityClaimType> UpdateAsync(IdentityClaimType claimType)
        {
            if (await _identityClaimTypeRepository.AnyAsync(claimType.Name, claimType.Id))
            {
                throw new AbpException($"Name Exist: {claimType.Name}");
            }

            if (claimType.IsStatic)
            {
                throw new AbpException($"Can not update a static ClaimType.");
            }
            

            return await _identityClaimTypeRepository.UpdateAsync(claimType);
        }
    }
}
