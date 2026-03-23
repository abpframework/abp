using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.PermissionManagement;

[DependsOn(
    typeof(AbpPermissionManagementDomainModule),
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule)
    )]
public class AbpPermissionManagementTestBaseModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(ServiceDescriptor.Singleton<IAbpDistributedLock, NullAbpDistributedLock>());

        context.Services.Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<TestPermissionManagementProvider>();
            options.ResourceManagementProviders.Add<TestResourcePermissionManagementProvider>();
            options.ResourceManagementProviders.Add<TestUnavailableResourcePermissionManagementProvider>();
            options.ResourcePermissionProviderKeyLookupServices.Add<TestResourcePermissionProviderKeyLookupService>();
            options.ResourcePermissionProviderKeyLookupServices.Add<TestUnavailableResourcePermissionProviderKeyLookupService>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            AsyncHelper.RunSync(() => scope.ServiceProvider
                .GetRequiredService<PermissionTestDataBuilder>()
                .BuildAsync());
        }
    }
}
