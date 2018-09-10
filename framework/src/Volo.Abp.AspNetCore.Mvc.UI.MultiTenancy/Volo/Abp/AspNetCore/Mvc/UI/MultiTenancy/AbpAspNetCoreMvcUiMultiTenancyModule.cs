using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAspNetCoreMultiTenancyModule)
        )]
    public class AbpAspNetCoreMvcUiMultiTenancyModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(AbpUiMultiTenancyResource),
                    typeof(AbpAspNetCoreMvcUiMultiTenancyModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiMultiTenancyModule>();
            });

            Configure<ToolbarOptions>(options =>
            {
                options.Contributors.Add(new MultiTenancyToolbarContributor());
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpUiMultiTenancyResource>("en")
                    .AddVirtualJson("/Volo/Abp/AspNetCore/Mvc/UI/MultiTenancy/Localization");
            });

            Configure<BundlingOptions>(options =>
            {
                options.ScriptBundles
                    .Get(StandardBundles.Scripts.Global)
                    .AddFiles(
                        "/Pages/Abp/MultiTenancy/tenant-switch.js"
                    );
            });
        }
    }
}
