using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Contributors;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public static class BundleConfigurationExtensions
    {
        public static void AddFiles(this BundleConfiguration bundleConfiguration, params string[] files)
        {
            bundleConfiguration.Contributors.Add(new SimpleBundleContributor(files));
        }
    }
}
