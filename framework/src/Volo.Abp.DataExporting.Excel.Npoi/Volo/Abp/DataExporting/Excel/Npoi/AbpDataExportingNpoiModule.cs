using Volo.Abp.DataExporting.Excel.Core;
using Volo.Abp.Modularity;

namespace Volo.Abp.DataExporting.Excel.Npoi;

[DependsOn(
    typeof(AbpDataExportingExcelCoreModule)
)]
public class AbpDataExportingNpoiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}