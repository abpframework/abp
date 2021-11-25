using Volo.Abp.Bundling;

namespace Volo.Abp.Cli.Bundling;

public interface IBundler
{
    string Bundle(BundleOptions options, BundleContext context);
}
