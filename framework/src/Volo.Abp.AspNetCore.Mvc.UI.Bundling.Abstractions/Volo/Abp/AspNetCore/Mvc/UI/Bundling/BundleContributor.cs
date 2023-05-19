using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public abstract class BundleContributor : IBundleContributor
{
    public virtual Task PreConfigureBundleAsync(BundleConfigurationContext context)
    {
        PreConfigureBundle(context);
        return Task.CompletedTask;
    }

    public virtual void PreConfigureBundle(BundleConfigurationContext context)
    {

    }

    public virtual Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        ConfigureBundle(context);
        return Task.CompletedTask;
    }

    public virtual void ConfigureBundle(BundleConfigurationContext context)
    {

    }

    public virtual Task PostConfigureBundleAsync(BundleConfigurationContext context)
    {
        PostConfigureBundle(context);
        return Task.CompletedTask;
    }

    public virtual void PostConfigureBundle(BundleConfigurationContext context)
    {

    }

    public virtual Task ConfigureDynamicResourcesAsync(BundleConfigurationContext context)
    {
        ConfigureDynamicResources(context);
        return Task.CompletedTask;
    }

    public virtual void ConfigureDynamicResources(BundleConfigurationContext context)
    {

    }
}
