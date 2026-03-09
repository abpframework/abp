using Volo.Abp.AspNetCore;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpCachingModule),
    typeof(AbpLocalizationModule),
    typeof(AbpSecurityModule),
    typeof(AbpAspNetCoreAbstractionsModule),
    typeof(AbpDistributedLockingAbstractionsModule)
)]
public class AbpOperationRateLimitingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOperationRateLimitingModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpOperationRateLimitingResource>("en")
                .AddVirtualJson("/Volo/Abp/OperationRateLimiting/Localization");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(
                "Volo.Abp.OperationRateLimiting",
                typeof(AbpOperationRateLimitingResource));
        });
    }
}
