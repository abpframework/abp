using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.MultiTenancy;

namespace Pages.Abp.MultiTenancy;

public class AbpTenantAppService : ApplicationService, IAbpTenantAppService
{
    protected ITenantStore TenantStore { get; }
    protected ITenantNormalizer TenantNormalizer { get; }

    public AbpTenantAppService(ITenantStore tenantStore, ITenantNormalizer tenantNormalizer)
    {
        TenantStore = tenantStore;
        TenantNormalizer = tenantNormalizer;
    }

    public virtual async Task<FindTenantResultDto> FindTenantByNameAsync(string name)
    {
        var tenant = await TenantStore.FindAsync(TenantNormalizer.NormalizeName(name)!);

        if (tenant == null)
        {
            return new FindTenantResultDto { Success = false };
        }

        return new FindTenantResultDto
        {
            Success = true,
            TenantId = tenant.Id,
            Name = tenant.Name,
            NormalizedName = tenant.NormalizedName,
            IsActive = tenant.IsActive
        };
    }

    public virtual async Task<FindTenantResultDto> FindTenantByIdAsync(Guid id)
    {
        var tenant = await TenantStore.FindAsync(id);

        if (tenant == null)
        {
            return new FindTenantResultDto { Success = false };
        }

        return new FindTenantResultDto
        {
            Success = true,
            TenantId = tenant.Id,
            Name = tenant.Name,
            NormalizedName = tenant.NormalizedName,
            IsActive = tenant.IsActive
        };
    }
}
