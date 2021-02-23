using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.SettingManagement.Blazor.Settings;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.SettingManagement.Blazor
{
    [DependsOn(
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpSettingManagementHttpApiClientModule)
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
                options.Contributors.Add(new EmailSettingPageContributor());
            });
        }
    }
}
