using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Localization;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.PermissionManagement.Web;

[DependsOn(typeof(AbpPermissionManagementApplicationContractsModule))]
[DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
[DependsOn(typeof(AbpMapperlyModule))]
public class AbpPermissionManagementWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AbpPermissionManagementResource),
                typeof(AbpPermissionManagementWebModule).Assembly,
                typeof(AbpPermissionManagementApplicationContractsModule).Assembly
            );
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpPermissionManagementWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpPermissionManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPermissionManagementWebModule>();
        });

        context.Services.AddMapperlyObjectMapper<AbpPermissionManagementWebModule>();

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(PermissionManagementRemoteServiceConsts.ModuleName);
        });
    }
}
