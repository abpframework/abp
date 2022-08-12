using Volo.Abp.DataExporting.Excel.Core;
using Volo.Abp.Modularity;

namespace Volo.Abp.DataExporting.Excel.ClosedXML;

[DependsOn(
    typeof(AbpDataExportingExcelCoreModule)
)]
public class AbpDataExportingClosedXmlModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}