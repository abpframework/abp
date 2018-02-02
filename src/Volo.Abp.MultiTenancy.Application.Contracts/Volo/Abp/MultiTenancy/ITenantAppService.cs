using System;
using Volo.Abp.Application.Services;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, Guid, GetTenantsInput, TenantCreateDto, TenantUpdateDto>
    {
        //TODO: Manage connection strings
    }
}
