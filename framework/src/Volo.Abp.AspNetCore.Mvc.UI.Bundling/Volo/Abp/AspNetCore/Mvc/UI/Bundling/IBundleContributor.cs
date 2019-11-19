using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundleContributor
    {
        Task PreConfigureBundleAsync(BundleConfigurationContext context);

        Task ConfigureBundleAsync(BundleConfigurationContext context);

        Task PostConfigureBundleAsync(BundleConfigurationContext context);

        Task ConfigureDynamicResourcesAsync(BundleConfigurationContext context);
    }
}