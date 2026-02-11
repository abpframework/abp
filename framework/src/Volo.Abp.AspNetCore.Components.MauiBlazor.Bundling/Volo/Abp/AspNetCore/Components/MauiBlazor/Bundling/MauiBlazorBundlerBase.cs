using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Minify;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Bundling;

public abstract class MauiBlazorBundlerBase : BundlerBase
{
    protected IMauiBlazorContentFileProvider MauiBlazorContentFileProvider { get; }

    public MauiBlazorBundlerBase(
        IMauiBlazorContentFileProvider mauiBlazorContentFileProvider,
        IMinifier minifier,
        IOptions<AbpBundlingOptions> bundlingOptions) : base(minifier,
        bundlingOptions)
    {
        MauiBlazorContentFileProvider = mauiBlazorContentFileProvider;
    }

    protected override IFileInfo FindFileInfo(string file)
    {
        return MauiBlazorContentFileProvider.GetFileInfo(file);
    }
}