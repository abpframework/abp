using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.Libs;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Data;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBootstrapModule),
    typeof(AbpAspNetCoreBundlingModule)
    )]
public class AbpAspNetCoreMvcUiBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        if (!context.Services.IsDataMigrationEnvironment())
        {
            Configure<AbpMvcLibsOptions>(options =>
            {
                options.CheckLibs = true;
            });
        }
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var environment = context.GetEnvironmentOrNull();
        if (environment != null)
        {
            environment.WebRootFileProvider =
                new CompositeFileProvider(
                    context.GetEnvironment().WebRootFileProvider,
                    context.ServiceProvider.GetRequiredService<IWebContentFileProvider>()
                );
        }

        await InitialGlobalAssetsAsync(context);
    }

    protected virtual async Task InitialGlobalAssetsAsync(ApplicationInitializationContext context)
    {
        var bundlingOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpBundlingOptions>>().Value;
        var logger = context.ServiceProvider.GetRequiredService<ILogger<AbpAspNetCoreMvcUiBundlingModule>>();
        if (!bundlingOptions.GlobalAssets.Enabled)
        {
            return;
        }

        var bundleManager = context.ServiceProvider.GetRequiredService<BundleManager>();
        var webHostEnvironment = context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var dynamicFileProvider = context.ServiceProvider.GetRequiredService<IDynamicFileProvider>();
        if (!bundlingOptions.GlobalAssets.GlobalStyleBundleName.IsNullOrWhiteSpace())
        {
            var styleFiles = await bundleManager.GetStyleBundleFilesAsync(bundlingOptions.GlobalAssets.GlobalStyleBundleName);
            var styles = string.Empty;
            foreach (var file in styleFiles)
            {
                var fileInfo = webHostEnvironment.WebRootFileProvider?.GetFileInfo(file.FileName);
                if (fileInfo == null || !fileInfo.Exists)
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
                var fileInfo = webHostEnvironment.WebRootFileProvider?.GetFileInfo(file.FileName);
                if (fileInfo == null || !fileInfo.Exists)
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
