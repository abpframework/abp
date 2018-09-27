using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public interface IIdentityResourceRepository : IBasicRepository<IdentityResource, Guid>
    {
        Task<List<IdentityResource>> GetListByScopesAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityResource>> GetListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );
    }
}