namespace Volo.Abp.Bundling
{
    public interface IBundleContributor
    {
        void AddScripts(BundleContext context, BundleParameterDictionary parameters);
        void AddStyles(BundleContext context, BundleParameterDictionary parameters);
    }
}