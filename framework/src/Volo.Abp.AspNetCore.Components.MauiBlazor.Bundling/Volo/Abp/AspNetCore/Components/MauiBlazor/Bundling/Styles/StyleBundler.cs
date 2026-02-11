using System;
using System.IO;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Bundling.Styles;
using Volo.Abp.Minify.Styles;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Bundling.Styles;

public class StyleBundler : MauiBlazorBundlerBase, IStyleBundler
{
    private readonly IMauiBlazorContentFileProvider _mauiBlazorContentFileProvider;
    public override string FileExtension => "css";

    public StyleBundler(
        IMauiBlazorContentFileProvider mauiBlazorContentFileProvider,
        ICssMinifier minifier,
        IOptions<AbpBundlingOptions> bundlingOptions)
        : base(
            mauiBlazorContentFileProvider,
            minifier,
            bundlingOptions)
    {
        _mauiBlazorContentFileProvider = mauiBlazorContentFileProvider;
    }

    public string GetAbsolutePath(string relativePath)
    {
        return Path.Combine(_mauiBlazorContentFileProvider.ContentRootPath, "wwwroot", relativePath.RemovePreFix("/")).Replace("file://", "");
    }

    protected override string ProcessBeforeAddingToTheBundle(IBundlerContext context, string filePath, string fileContent)
    {
        return CssRelativePath.Adjust(
            fileContent,
            GetAbsolutePath(filePath),
            GetAbsolutePath(context.BundleRelativePath)
        );
    }
}