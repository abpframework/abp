using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.FileProviders;
using Microsoft.Maui;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Bundling;

public class AbpBlazorWebView : BlazorWebView
{
    public override IFileProvider CreateFileProvider(string contentRootDir)
    {
        return new CompositeFileProvider(Handler!.GetRequiredService<IMauiBlazorContentFileProvider>(), base.CreateFileProvider(contentRootDir));
    }
}