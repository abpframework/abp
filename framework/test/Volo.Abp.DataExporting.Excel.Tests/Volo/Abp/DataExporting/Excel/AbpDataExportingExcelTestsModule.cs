using Volo.Abp.DataExporting.Excel.ClosedXML;
using Volo.Abp.DataExporting.Excel.EPPlus;
using Volo.Abp.DataExporting.Excel.Npoi;
using Volo.Abp.Modularity;

namespace Volo.Abp.DataExporting.Excel;

[DependsOn(
    typeof(AbpDataExportingClosedXmlModule),
    typeof(AbpDataExportingEpplusModule),
    typeof(AbpDataExportingNpoiModule)
    )]

public class AbpDataExportingExcelTestsModule : AbpModule
{
    
}