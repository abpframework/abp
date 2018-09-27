using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(typeof(AbpIdentityServerDomainSharedModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpIdentityDomainModule))]
    [DependsOn(typeof(AbpSecurityModule))]
    public class AbpIdentityServerDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<ClientAutoMapperProfile>(validate: true);
            });

            AddIdentityServer(context.Services);
        }

        private static void AddIdentityServer(IServiceCollection services)
        {
            var identityServerBuilder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            });

            identityServerBuilder
                .AddDeveloperSigningCredential() //TODO: Should be able to change this!
                .AddClientStore<ClientStore>()
                .AddResourceStore<ResourceStore>()
                .AddAbpIdentityServer();

            services.ExecutePreConfiguredActions(identityServerBuilder);
        }
    }
}
