﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.FeatureManagement;

[DependsOn(
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule)
    )]
public class AbpFeatureManagementWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AbpFeatureManagementResource), typeof(AbpFeatureManagementWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpFeatureManagementWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpFeatureManagementWebModule>();
        });

        Configure<RazorPagesOptions>(options =>
        {
                //Configure authorization.
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(FeatureManagementRemoteServiceConsts.ModuleName);
        });
    }
}
