using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity
{
    public interface IIdentityClaimTypeRepository : IBasicRepository<IdentityClaimType, Guid>
    {
        Task<bool> DoesNameExist(string name, Guid? claimTypeId = null);

        Task<List<IdentityClaimType>> GetListAsync(string sorting, int maxResultCount, int skipCount);

        Task<int> GetTotalCount();
    }
}
