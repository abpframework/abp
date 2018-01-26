using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public interface IApiResourceRepository : IBasicRepository<ApiResource, Guid>
    {
        Task<ApiResource> FindByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}