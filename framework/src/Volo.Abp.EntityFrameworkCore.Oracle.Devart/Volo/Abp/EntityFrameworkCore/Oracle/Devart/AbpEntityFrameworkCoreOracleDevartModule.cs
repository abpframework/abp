using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.Oracle.Devart
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCoreOracleDevartModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                if (options.DefaultSequentialGuidType == null)
                {
                    options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsBinary;
                }
            });
        }
    }
}
