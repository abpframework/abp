using JetBrains.Annotations;

namespace Volo.Abp.Cli.Bundling;

public class BundleOptions
{
    [NotNull]
    public string Directory { get; set; }

    [NotNull]
    public string BundleName { get; set; }

    [NotNull]
    public string FrameworkVersion { get; set; }

    [NotNull]
    public string ProjectFileName { get; set; }

    public bool Minify { get; set; }
}
