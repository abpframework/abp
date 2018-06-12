namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public abstract class BundleContributor : IBundleContributor
    {
        public abstract void ConfigureBundle(BundleConfigurationContext context);
    }
}