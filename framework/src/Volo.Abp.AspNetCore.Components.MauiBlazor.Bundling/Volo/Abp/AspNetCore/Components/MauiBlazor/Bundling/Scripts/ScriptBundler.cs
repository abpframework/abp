using System;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Scripts;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Minify.Scripts;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Bundling.Scripts;

public class ScriptBundler : MauiBlazorBundlerBase, IScriptBundler
{
    public override string FileExtension => "js";

    public ScriptBundler(
        IMauiBlazorContentFileProvider mauiBlazorContentFileProvider,
        IJavascriptMinifier minifier,
        IOptions<AbpBundlingOptions> bundlingOptions)
        : base(
            mauiBlazorContentFileProvider,
            minifier,
            bundlingOptions)
    {
    }

    protected override string ProcessBeforeAddingToTheBundle(IBundlerContext context, string filePath, string fileContent)
    {
        return fileContent.EnsureEndsWith(';') + Environment.NewLine;
    }
}