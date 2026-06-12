using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.IdentityServer;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpIdentityServerDomainModule)
)]
public class AbpIdentityServerTestBaseModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityServerBuilderOptions>(options =>
        {
            options.AddDeveloperSigningCredential = false;
        });

        PreConfigure<IIdentityServerBuilder>(identityServerBuilder =>
        {
            identityServerBuilder.AddDeveloperSigningCredential(false, System.Guid.NewGuid().ToString());
        });
    }
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            AsyncHelper.RunSync(async () =>
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                using (var uow = unitOfWorkManager.Begin(requiresNew: true))
                {
                    await scope.ServiceProvider
                        .GetRequiredService<AbpIdentityServerTestDataBuilder>()
                        .BuildAsync();
                    await uow.CompleteAsync();
                }
            });
        }
    }
}
