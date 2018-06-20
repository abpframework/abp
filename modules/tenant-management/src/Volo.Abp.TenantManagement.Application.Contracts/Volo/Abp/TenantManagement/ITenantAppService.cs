using System;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TenantManagement
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, Guid, GetTenantsInput, TenantCreateDto, TenantUpdateDto>
    {
        //TODO: Manage connection strings
    }
}
