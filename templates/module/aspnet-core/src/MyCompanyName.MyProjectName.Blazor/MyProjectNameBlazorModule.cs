using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Blazor.Menus;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace MyCompanyName.MyProjectName.Blazor
{
    [DependsOn(
        typeof(MyProjectNameHttpApiClientModule),
        typeof(AbpAutoMapperModule)
        )]
    public class MyProjectNameBlazorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<MyProjectNameBlazorModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MyProjectNameBlazorAutoMapperProfile>(validate: true);
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new MyProjectNameMenuContributor());
            });

            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(MyProjectNameBlazorModule).Assembly);
            });
        }
    }
}