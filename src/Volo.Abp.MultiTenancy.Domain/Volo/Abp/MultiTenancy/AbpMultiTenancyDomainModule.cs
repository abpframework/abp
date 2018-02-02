using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Ui;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    [DependsOn(typeof(AbpMultiTenancyDomainSharedModule))]
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpDddModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpUiModule))] //TODO: It's not good to depend on the UI module. However, UserFriendlyException is inside it!
    public class AbpMultiTenancyDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpMultiTenancyDomainMappingProfile>(validate: true);
            });

            services.AddAssemblyOf<AbpMultiTenancyDomainModule>();
        }
    }
}
