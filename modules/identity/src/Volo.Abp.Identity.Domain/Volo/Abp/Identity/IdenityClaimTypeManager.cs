using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace Volo.Abp.Identity
{
    public class IdenityClaimTypeManager : IDomainService
    {
        private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;
        private readonly IGuidGenerator _guidGenerator;

        public IdenityClaimTypeManager(IIdentityClaimTypeRepository identityClaimTypeRepository, IGuidGenerator guidGenerator)
        {
            _identityClaimTypeRepository = identityClaimTypeRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<IdentityClaimType> GetAsync(Guid id)
        {
            return await _identityClaimTypeRepository.GetAsync(id);
        }

        public async Task<IdentityClaimType> CreateAsync(IdentityClaimType claimType)
        {
            if (await _identityClaimTypeRepository.DoesNameExist(claimType.Name))
            {
                throw new AbpException($"Name Exist: {claimType.Name}");
            }

            return await _identityClaimTypeRepository.InsertAsync(claimType);
        }

        public async Task<IdentityClaimType> UpdateAsync(IdentityClaimType claimType)
        {
            if (await _identityClaimTypeRepository.DoesNameExist(claimType.Name, claimType.Id))
            {
                throw new AbpException($"Name Exist: {claimType.Name}");
            }

            return await _identityClaimTypeRepository.UpdateAsync(claimType);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _identityClaimTypeRepository.DeleteAsync(id);
        }
    }
}
