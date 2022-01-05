using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class AbpBundlingOptions
{
    public BundleConfigurationCollection StyleBundles { get; }

    public BundleConfigurationCollection ScriptBundles { get; }

    public HashSet<string> MinificationIgnoredFiles { get; }

    /// <summary>
    /// Default: "__bundles".
    /// </summary>
    public string BundleFolderName { get; } = "__bundles";

    /// <summary>
    /// Default: auto.
    /// </summary>
    public BundlingMode Mode { get; set; } = BundlingMode.Auto;

    public bool DeferScriptsByDefault { get; set; }

    public List<string> DeferScripts { get; }

    public bool PreloadStylesByDefault { get; set; }

    public List<string> PreloadStyles { get; }

    public AbpBundlingOptions()
    {
        StyleBundles = new BundleConfigurationCollection();
        ScriptBundles = new BundleConfigurationCollection();
        MinificationIgnoredFiles = new HashSet<string>();
        DeferScriptsByDefault = false;
        DeferScripts = new List<string>();
        PreloadStylesByDefault = false;
        PreloadStyles = new List<string>();
    }
}
