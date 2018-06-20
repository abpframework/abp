using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.PermissionManagement
{
    public interface IPermissionGrantRepository : IBasicRepository<PermissionGrant, Guid>
    {
        Task<PermissionGrant> FindAsync(string name, string providerName, string providerKey);

        Task<List<PermissionGrant>> GetListAsync(string providerName, string providerKey);
    }
}