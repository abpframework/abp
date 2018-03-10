using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.AutoMapper
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpCommonModule))]
    public class AutoMapperTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.UseStaticMapper = false;
            });

            services.AddAssemblyOf<AutoMapperTestModule>();
        }
    }
}