using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy;

public class UpperInvariantTenantNormalizer : ITenantNormalizer, ITransientDependency
{
    public virtual string? NormalizeName(string? name)
    {
        return name?.Normalize().ToUpperInvariant();
    }
}
