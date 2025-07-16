using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TenantManagement;

public class AbpTenantValidator : ITenantValidator, ITransientDependency
{
    protected ITenantRepository TenantRepository { get; }

    public AbpTenantValidator(ITenantRepository tenantRepository)
    {
        TenantRepository = tenantRepository;
    }

    public virtual async Task ValidateAsync(Tenant tenant)
    {
        Check.NotNullOrWhiteSpace(tenant.Name, nameof(tenant.Name));
        Check.NotNullOrWhiteSpace(tenant.NormalizedName, nameof(tenant.NormalizedName));

        var owner = await TenantRepository.FindByNameAsync(tenant.NormalizedName);
        if (owner != null && owner.Id != tenant.Id)
        {
            throw new BusinessException("Volo.Abp.TenantManagement:DuplicateTenantName").WithData("Name", tenant.NormalizedName);
        }
    }
}
