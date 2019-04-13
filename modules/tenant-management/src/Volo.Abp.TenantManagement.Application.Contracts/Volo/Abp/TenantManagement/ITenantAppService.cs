using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TenantManagement
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, Guid, GetTenantsInput, TenantCreateDto, TenantUpdateDto>
    {
        Task<string> GetDefaultConnectionStringAsync(Guid id);

        Task SetDefaultConnectionStringAsync(Guid id, string defaultConnectionString);

        Task RemoveDefaultConnectionStringAsync(Guid id);

    }
}
