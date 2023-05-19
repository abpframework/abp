using System.Threading.Tasks;

namespace Volo.Abp.MultiTenancy;

public interface ITenantResolveContributor
{
    string Name { get; }

    Task ResolveAsync(ITenantResolveContext context);
}
