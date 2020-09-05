using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.BlazoriseUI;

namespace Volo.Abp.Identity.Blazor
{
    [DependsOn(
        typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpBlazoriseUIModule),
        typeof(AbpAutoMapperModule)
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
        }
    }
}
