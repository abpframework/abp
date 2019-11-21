namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public abstract class BundleContributor
    {
        public virtual void PreConfigureBundle(BundleConfigurationContext context)
        {

        }

        public virtual void ConfigureBundle(BundleConfigurationContext context)
        {

        }

        public virtual void PostConfigureBundle(BundleConfigurationContext context)
        {

        }

        //TODO: Reconsider naming and usage!
        public virtual void ConfigureDynamicResources(BundleConfigurationContext context)
        {

        }
    }
}