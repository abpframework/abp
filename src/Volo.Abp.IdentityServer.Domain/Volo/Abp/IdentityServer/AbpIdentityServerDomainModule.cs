using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(typeof(AbpIdentityServerDomainSharedModule))]
    [DependsOn(typeof(AbpDddModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpIdentityServerDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<ClientAutoMapperProfile>(/*TODO: validate: true*/);
            });

            services.AddAssemblyOf<AbpIdentityServerDomainModule>();
        }
    }
}
