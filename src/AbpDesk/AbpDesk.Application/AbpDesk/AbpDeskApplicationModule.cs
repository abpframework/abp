using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    [DependsOn(
        typeof(AbpDeskDomainModule), 
        typeof(AbpDeskApplicationContractsModule), 
        typeof(AbpAutoMapperModule)
        )]
    public class AbpDeskApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpDeskApplicationModule>();

            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.Configurators.Add(context =>
                {
                    context.MapperConfiguration.AddProfile<AbpDeskApplicationModuleAutoMapperProfile>();
                });
            });
        }
    }
}
