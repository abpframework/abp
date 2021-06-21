using Volo.Abp.ExceptionHandling.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.ExceptionHandling
{
    /* TODO: This package is introduced in a later time by gathering
     * classes from multiple packages.
     * So, didn't change the original namespaces of the types to not introduce breaking changes.
     * We will re-design this package in a later time, probably with v5.0.
     */
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class AbpExceptionHandlingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpExceptionHandlingModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpExceptionHandlingResource>("en")
                    .AddVirtualJson("/Volo/Abp/ExceptionHandling/Localization");
            });
        }
    }
}
