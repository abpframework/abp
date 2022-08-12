using Volo.Abp.DataExporting.Excel.Core;
using Volo.Abp.Modularity;

namespace Volo.Abp.DataExporting.Excel.EPPlus;

[DependsOn(
    typeof(AbpDataExportingExcelCoreModule)
)]
public class AbpDataExportingEpplusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}