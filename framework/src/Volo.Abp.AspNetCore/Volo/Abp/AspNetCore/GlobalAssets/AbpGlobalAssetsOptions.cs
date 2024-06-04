using System;
using System.Collections.Generic;
using System.Linq;
namespace Volo.Abp.AspNetCore.GlobalAssets;

public class AbpGlobalAssetsOptions
{
    private static string[] _minFileSuffixes = { "min", "prod" };

    public Type? StartupModuleType { get; set; }

    public bool Minify { get; set; }

    public List<string> MinificationIgnoredFiles { get; set; }

    public string JavaScriptFileName { get; set; }

    public string CssFileName { get; set; }

    public AbpGlobalAssetsOptions()
    {
        Minify = true;
        MinificationIgnoredFiles = new List<string>
        {
            "_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js"
        };
        JavaScriptFileName = "global.js";
        CssFileName = "global.css";
    }

    public bool IsMinificationIgnored(string fileName)
    {
        return MinificationIgnoredFiles.Contains(fileName, StringComparer.OrdinalIgnoreCase) ||
               _minFileSuffixes.Any(suffix => fileName.EndsWith("." + suffix + ".js", StringComparison.OrdinalIgnoreCase)) ||
               _minFileSuffixes.Any(suffix => fileName.EndsWith("." + suffix + ".css", StringComparison.OrdinalIgnoreCase));
    }
}
