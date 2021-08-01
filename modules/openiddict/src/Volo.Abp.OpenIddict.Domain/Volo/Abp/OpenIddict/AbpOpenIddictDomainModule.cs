using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpOpenIddictDomainSharedModule)
    )]
    public class AbpOpenIddictDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.Configure<AbpOpenIddictOptions>(configuration.GetSection("OpenIddict"));
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            AddOpenIddict(context.Services);
        }

        private static void AddOpenIddict(IServiceCollection services)
        {
            var openIddictBuilder = services.AddOpenIddict();

            openIddictBuilder.AddAbpOpenIddict();

            var openIddictCoreBuilder = openIddictBuilder.AddAbpOpenIddictCore();

            services.ExecutePreConfiguredActions(openIddictBuilder);

            services.ExecutePreConfiguredActions(openIddictCoreBuilder);

            openIddictCoreBuilder.TryAddAbpMemoryStore();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<OpenIddictCleanupOptions>>().Value;
            var backgroundWorkerManager = context.ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();
            if (options.IsCleanupAuthorizationEnabled)
            {
                backgroundWorkerManager.Add(
                    context.ServiceProvider
                        .GetRequiredService<OpenIddictAuthorizationCleanupBackgroundWorker>()
                );
            }
            if (options.IsCleanupTokenEnabled)
            {
                backgroundWorkerManager.Add(
                    context.ServiceProvider
                        .GetRequiredService<OpenIddictTokenCleanupBackgroundWorker>()
                );
            }
        }
    }
}
