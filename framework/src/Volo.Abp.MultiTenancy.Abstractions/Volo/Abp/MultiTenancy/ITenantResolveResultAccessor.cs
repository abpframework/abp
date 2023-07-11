using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy;

public interface ITenantResolveResultAccessor
{
    TenantResolveResult? Result { get; set; }
}
