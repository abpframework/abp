using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.MultiTenancy
{
    public interface IAbpTenantAppService : IApplicationService
    {
        Task<FindTenantResultDto> FindTenantByNameAsync(string name);

        Task<FindTenantResultDto> FindTenantByIdAsync(Guid id);
    }
}