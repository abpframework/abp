namespace Volo.Abp.Bundling
{
    public interface IBundleContributor
    {
        void AddScripts(BundleContext context);
        void AddStyles(BundleContext context);
    }
}