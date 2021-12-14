using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.SettingManagement.Blazor.Settings;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.SettingManagement.Blazor;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpSettingManagementApplicationContractsModule)
)]
public class AbpSettingManagementBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpSettingManagementBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<SettingManagementBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new SettingManagementMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpSettingManagementBlazorModule).Assembly);
        });

        Configure<SettingManagementComponentOptions>(options =>
        {
            options.Contributors.Add(new EmailingPageContributor());
        });
    }
}
