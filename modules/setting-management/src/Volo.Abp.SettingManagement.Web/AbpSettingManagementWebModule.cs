﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.SettingManagement.Web.Settings;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.SettingManagement.Web;

[DependsOn(
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpSettingManagementDomainSharedModule)
    )]
public class AbpSettingManagementWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AbpSettingManagementResource), typeof(AbpSettingManagementWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpSettingManagementWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new SettingManagementMainMenuContributor());
        });

        Configure<SettingManagementPageOptions>(options =>
        {
            options.Contributors.Add(new EmailingPageContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpSettingManagementWebModule>();
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(SettingManagementRemoteServiceConsts.ModuleName);
        });

        context.Services.AddAutoMapperObjectMapper<AbpSettingManagementWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<SettingManagementWebAutoMapperProfile>(validate: true);
        });
    }
}
