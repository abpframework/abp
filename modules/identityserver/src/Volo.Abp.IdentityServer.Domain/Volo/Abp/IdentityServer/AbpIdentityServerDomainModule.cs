using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpIdentityServerDomainSharedModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpSecurityModule)
        )]
    public class AbpIdentityServerDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<ClientAutoMapperProfile>(validate: true);
            });

            AddIdentityServer(context.Services);
        }
        
        private static void AddIdentityServer(IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            var builderOptions = services.ExecutePreConfiguredActions<AbpIdentityServerBuilderOptions>();

            var identityServerBuilder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            });

            identityServerBuilder
                .AddDeveloperSigningCredential() //TODO: Should be able to change this!
                .AddAbpIdentityServer(builderOptions);

            services.ExecutePreConfiguredActions(identityServerBuilder);

            if (!services.IsAdded<IPersistedGrantService>())
            {
                identityServerBuilder.AddInMemoryPersistedGrants();
            }

            if (!services.IsAdded<IClientStore>())
            {
                identityServerBuilder.AddInMemoryClients(configuration.GetSection("IdentityServer:Clients"));
            }

            if (!services.IsAdded<IResourceStore>())
            {
                identityServerBuilder.AddInMemoryApiResources(configuration.GetSection("IdentityServer:ApiResources"));
                identityServerBuilder.AddInMemoryIdentityResources(configuration.GetSection("IdentityServer:IdentityResources"));
            }
        }
    }
}
