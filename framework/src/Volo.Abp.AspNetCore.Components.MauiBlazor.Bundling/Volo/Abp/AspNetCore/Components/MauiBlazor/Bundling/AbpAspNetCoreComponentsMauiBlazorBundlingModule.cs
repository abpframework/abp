using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Bundling.Styles;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreComponentsMauiBlazorModule),
    typeof(AbpAspNetCoreBundlingModule)
)]
public class AbpAspNetCoreComponentsMauiBlazorBundlingModule : AbpModule
{
	public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await InitialGlobalAssetsAsync(context);
    }

    protected virtual async Task InitialGlobalAssetsAsync(ApplicationInitializationContext context)
    {
        var bundlingOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpBundlingOptions>>().Value;
        var logger = context.ServiceProvider.GetRequiredService<ILogger<AbpAspNetCoreComponentsMauiBlazorBundlingModule>>();
        if (!bundlingOptions.GlobalAssets.Enabled)
        {
            return;
        }

        var bundleManager = context.ServiceProvider.GetRequiredService<BundleManager>();
        var mauiBlazorContentFileProvider = context.ServiceProvider.GetRequiredService<IMauiBlazorContentFileProvider>();
        var dynamicFileProvider = context.ServiceProvider.GetRequiredService<IDynamicFileProvider>();
        if (!bundlingOptions.GlobalAssets.GlobalStyleBundleName.IsNullOrWhiteSpace())
        {
            var styleFiles = await bundleManager.GetStyleBundleFilesAsync(bundlingOptions.GlobalAssets.GlobalStyleBundleName);
            var styles = string.Empty;
            foreach (var file in styleFiles)
            {
                var fileInfo = mauiBlazorContentFileProvider.GetFileInfo(file.FileName);
                if (!fileInfo.Exists)
                {
                    logger.LogError($"Could not find the file: {file.FileName}");
                    continue;
                }

                var fileContent = await fileInfo.ReadAsStringAsync();
                if (!bundleManager.IsBundlingEnabled())
                {
                    fileContent = CssRelativePath.Adjust(fileContent,
                        file.FileName,
                        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));

                    styles += $"/*{file.FileName}*/{Environment.NewLine}{fileContent}{Environment.NewLine}{Environment.NewLine}";
                }
                else
                {
                    styles += $"{fileContent}{Environment.NewLine}{Environment.NewLine}";
                }
            }

            dynamicFileProvider.AddOrUpdate(
                new InMemoryFileInfo("/wwwroot/" + bundlingOptions.GlobalAssets.CssFileName,
                    Encoding.UTF8.GetBytes(styles),
                    bundlingOptions.GlobalAssets.CssFileName));
        }

        if (!bundlingOptions.GlobalAssets.GlobalScriptBundleName.IsNullOrWhiteSpace())
        {
            var scriptFiles = await bundleManager.GetScriptBundleFilesAsync(bundlingOptions.GlobalAssets.GlobalScriptBundleName);
            var scripts = string.Empty;
            foreach (var file in scriptFiles)
            {
                var fileInfo = mauiBlazorContentFileProvider.GetFileInfo(file.FileName);
                if (!fileInfo.Exists)
                {
                    logger.LogError($"Could not find the file: {file.FileName}");
                    continue;
                }

                var fileContent = await fileInfo.ReadAsStringAsync();
                if (!bundleManager.IsBundlingEnabled())
                {
                    scripts += $"{fileContent.EnsureEndsWith(';')}{Environment.NewLine}{Environment.NewLine}";
                }
                else
                {
                    scripts += $"//{file.FileName}{Environment.NewLine}{fileContent.EnsureEndsWith(';')}{Environment.NewLine}{Environment.NewLine}";
                }
            }

            dynamicFileProvider.AddOrUpdate(
                new InMemoryFileInfo("/wwwroot/" + bundlingOptions.GlobalAssets.JavaScriptFileName,
                    Encoding.UTF8.GetBytes(scripts),
                    bundlingOptions.GlobalAssets.JavaScriptFileName));
        }
    }
}