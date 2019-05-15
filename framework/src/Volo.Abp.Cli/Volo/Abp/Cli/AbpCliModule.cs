using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(AbpCliCoreModule),
        typeof(AbpAutofacModule)
    )]
    public class AbpCliModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
#if DEBUG
            Configure<CliOptions>(options =>
            {
                options.AbpIoWwwUrlRoot = "https://localhost:44328/";
                options.AbpIoAccountUrlRoot = "https://localhost:44333/";
            });
#endif
        }
    }
}