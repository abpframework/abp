namespace Volo.Abp.Bundling
{
    public interface IBundleContributer
    {
        void AddScripts(BundleContext context);
        void AddStyles(BundleContext context);
    }
}
