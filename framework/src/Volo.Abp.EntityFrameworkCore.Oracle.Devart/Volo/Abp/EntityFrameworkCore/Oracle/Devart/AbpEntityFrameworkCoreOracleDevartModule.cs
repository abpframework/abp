using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.Oracle.Devart
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCoreOracleDevartModule : AbpModule
    {
        
    }
}