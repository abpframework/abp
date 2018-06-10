namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundleContributor
    {
        void ConfigureBundle(BundleConfigurationContext context);
    }
}
