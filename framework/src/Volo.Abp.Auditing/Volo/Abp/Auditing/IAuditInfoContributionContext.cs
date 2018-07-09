using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing
{
    public interface IAuditInfoContributionContext : IServiceProviderAccessor
    {
        AuditInfo AuditInfo { get; set; }
    }
}