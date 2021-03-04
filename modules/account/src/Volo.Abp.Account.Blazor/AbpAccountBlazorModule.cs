using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.UI.Theming;
using Volo.Abp.AspNetCore.Components.UI.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Account.Blazor
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsUiThemingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAccountApplicationContractsModule)
        )]
    public class AbpAccountBlazorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpAccountBlazorModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpAccountBlazorAutoMapperProfile>(validate: true);
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpAccountBlazorUserMenuContributor());
            });

            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(AbpAccountBlazorModule).Assembly);
            });
        }
    }
}
