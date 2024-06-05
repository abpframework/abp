namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class AbpBundlingGlobalAssetsOptions
{
    public bool Enabled { get; set; }

    public string? GlobalStyleBundleName { get; set; }

    public string? GlobalScriptBundleName { get; set; }

    public string JavaScriptFileName { get; set; }

    public string CssFileName { get; set; }

    public AbpBundlingGlobalAssetsOptions()
    {
        JavaScriptFileName = "global.js";
        CssFileName = "global.css";
    }
}
