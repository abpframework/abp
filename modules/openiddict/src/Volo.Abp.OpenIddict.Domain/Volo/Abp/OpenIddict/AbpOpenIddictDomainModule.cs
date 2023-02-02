using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Threading;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpOpenIddictDomainSharedModule),
    typeof(AbpDistributedLockingAbstractionsModule),
    typeof(AbpCachingModule),
    typeof(AbpGuidsModule)
)]
public class AbpOpenIddictDomainModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AddOpenIddictCore(context.Services);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<TokenCleanupOptions>>().Value;
        if (options.IsCleanupEnabled)
        {
            await context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .AddAsync(context.ServiceProvider.GetRequiredService<TokenCleanupBackgroundWorker>());
        }
    }

    private void AddOpenIddictCore(IServiceCollection services)
    {
        var openIddictBuilder = services.AddOpenIddict()
            .AddCore(builder =>
            {
                builder
                    .SetDefaultApplicationEntity<OpenIddictApplicationModel>()
                    .SetDefaultAuthorizationEntity<OpenIddictAuthorizationModel>()
                    .SetDefaultScopeEntity<OpenIddictScopeModel>()
                    .SetDefaultTokenEntity<OpenIddictTokenModel>();

                builder
                    .AddApplicationStore<AbpOpenIddictApplicationStore>()
                    .AddAuthorizationStore<AbpOpenIddictAuthorizationStore>()
                    .AddScopeStore<AbpOpenIddictScopeStore>()
                    .AddTokenStore<AbpOpenIddictTokenStore>();

                builder.ReplaceApplicationManager(typeof(AbpApplicationManager));
                builder.ReplaceAuthorizationManager(typeof(AbpAuthorizationManager));
                builder.ReplaceScopeManager(typeof(AbpScopeManager));
                builder.ReplaceTokenManager(typeof(AbpTokenManager));

                builder.Services.TryAddScoped(provider => (IAbpApplicationManager)provider.GetRequiredService<IOpenIddictApplicationManager>());

                services.ExecutePreConfiguredActions(builder);
            });

        services.ExecutePreConfiguredActions(openIddictBuilder);
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                OpenIddictModuleExtensionConsts.ModuleName,
                OpenIddictModuleExtensionConsts.EntityNames.Application,
                typeof(OpenIddictApplication)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                OpenIddictModuleExtensionConsts.ModuleName,
                OpenIddictModuleExtensionConsts.EntityNames.Authorization,
                typeof(OpenIddictAuthorization)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                OpenIddictModuleExtensionConsts.ModuleName,
                OpenIddictModuleExtensionConsts.EntityNames.Scope,
                typeof(OpenIddictScope)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                OpenIddictModuleExtensionConsts.ModuleName,
                OpenIddictModuleExtensionConsts.EntityNames.Token,
                typeof(OpenIddictToken)
            );
        });
    }
}
