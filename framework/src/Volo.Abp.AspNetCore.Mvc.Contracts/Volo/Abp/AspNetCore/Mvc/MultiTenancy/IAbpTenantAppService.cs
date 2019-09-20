using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.MultiTenancy
{
    public interface IAbpTenantAppService : IApplicationService
    {
        Task<FindTenantResult> FindTenantByNameAsync(string name);

        Task<FindTenantResult> FindTenantByIdAsync(Guid id);
    }
}