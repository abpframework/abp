using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Scripts;
using Volo.Abp.Minify.Scripts;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Scripts;

public class ScriptBundler : MvcUiBundlerBase, IScriptBundler
{
    public override string FileExtension => "js";

    public ScriptBundler(
        IWebHostEnvironment hostingEnvironment,
        IJavascriptMinifier minifier,
        IOptions<AbpBundlingOptions> bundlingOptions)
        : base(
            hostingEnvironment,
            minifier,
            bundlingOptions)
    {
    }

    protected override string ProcessBeforeAddingToTheBundle(IBundlerContext context, string filePath, string fileContent)
    {
        return fileContent.EnsureEndsWith(';') + Environment.NewLine;
    }
}
