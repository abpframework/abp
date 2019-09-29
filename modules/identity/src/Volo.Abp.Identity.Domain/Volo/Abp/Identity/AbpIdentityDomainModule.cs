using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpIdentityDomainSharedModule),
        typeof(AbpUsersDomainModule)
        )]
    public class AbpIdentityDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<DistributedEventBusOptions>(options =>
            {
                options.EtoMappings.Add<IdentityUser, UserEto>();
            });

            var identityBuilder = context.Services.AddAbpIdentity(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            context.Services.AddObjectAccessor(identityBuilder);
            context.Services.ExecutePreConfiguredActions(identityBuilder);

            AddAbpIdentityOptionsFactory(context.Services);
        }

        private static void AddAbpIdentityOptionsFactory(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Transient<IOptionsFactory<IdentityOptions>, AbpIdentityOptionsFactory>());
            services.Replace(ServiceDescriptor.Scoped<IOptions<IdentityOptions>, OptionsManager<IdentityOptions>>());
        }
    }
}