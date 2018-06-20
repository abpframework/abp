using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Identity
{
    public interface IIdentityDataSeeder
    {
        Task SeedAsync(
            string adminUserPassword,
            IEnumerable<string> adminRolePermissions = null,
            Guid? tenantId = null);
    }
}