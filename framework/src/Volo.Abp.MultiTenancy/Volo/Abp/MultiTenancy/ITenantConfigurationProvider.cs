using System.Threading.Tasks;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantConfigurationProvider
    {
        Task<TenantConfiguration> GetAsync(bool saveResolveResult = false);
    }
}
