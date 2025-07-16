using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TenantManagement;

public class TenantManager : DomainService, ITenantManager
{
    protected ITenantValidator TenantValidator { get; }
    protected ITenantNormalizer TenantNormalizer { get; }
    protected ILocalEventBus LocalEventBus { get; }

    public TenantManager(
        ITenantValidator tenantValidator,
        ITenantNormalizer tenantNormalizer,
        ILocalEventBus localEventBus)
    {
        TenantValidator = tenantValidator;
        TenantNormalizer = tenantNormalizer;
        LocalEventBus = localEventBus;
    }

    public virtual async Task<Tenant> CreateAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var tenant = new Tenant(GuidGenerator.Create(), name, TenantNormalizer.NormalizeName(name));
        await TenantValidator.ValidateAsync(tenant);
        return tenant;
    }

    public virtual async Task ChangeNameAsync(Tenant tenant, string name)
    {
        Check.NotNull(tenant, nameof(tenant));
        Check.NotNull(name, nameof(name));

        await LocalEventBus.PublishAsync(new TenantChangedEvent(tenant.Id, tenant.NormalizedName));

        tenant.SetName(name);
        tenant.SetNormalizedName( TenantNormalizer.NormalizeName(name));
        await TenantValidator.ValidateAsync(tenant);
    }
}
