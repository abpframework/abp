using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.Data;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.CmsKit;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAuthorizationModule),
    typeof(CmsKitDomainModule)
    )]
public class CmsKitTestBaseModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // Set culture to InvariantCulture to avoid Turkish-I problem and other culture-specific issues in tests
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.CmsKit().EnableAll();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IBlobProvider>(Substitute.For<FakeBlobProvider>());

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.ProviderType = typeof(FakeBlobProvider);
            });
        });

        context.Services.AddAlwaysAllowAuthorization();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () =>
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                         .GetRequiredService<IDataSeeder>()
                         .SeedAsync();
            }
        });
    }
}
