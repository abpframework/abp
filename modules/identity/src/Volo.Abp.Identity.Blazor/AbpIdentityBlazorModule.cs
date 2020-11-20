using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Blazor;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Identity.Blazor
{
    [DependsOn(
        typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementBlazorModule),
        typeof(AbpBlazoriseUIModule)
        )]
    public class AbpIdentityBlazorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpIdentityBlazorModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpIdentityBlazorAutoMapperProfile>(validate: true);
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpIdentityWebMainMenuContributor());
            });

            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(AbpIdentityBlazorModule).Assembly);
            });
        }
    }
}
