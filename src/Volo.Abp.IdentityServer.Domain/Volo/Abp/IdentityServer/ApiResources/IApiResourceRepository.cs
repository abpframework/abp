using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public interface IApiResourceRepository : IRepository<ApiResource, Guid>
    {

    }
}