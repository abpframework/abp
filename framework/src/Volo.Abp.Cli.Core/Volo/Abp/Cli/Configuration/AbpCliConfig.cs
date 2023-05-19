using Volo.Abp.Cli.Bundling;

namespace Volo.Abp.Cli.Configuration;

public class AbpCliConfig
{
    public BundleConfig Bundle { get; set; } = new();
}
