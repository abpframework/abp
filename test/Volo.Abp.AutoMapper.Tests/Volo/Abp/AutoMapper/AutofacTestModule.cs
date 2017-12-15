using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AutoMapper
{
    [DependsOn(typeof(AbpAutoMapperModule))]
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