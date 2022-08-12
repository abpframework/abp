using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DataExporting.Excel.Core.Dto;
using Volo.Abp.Testing;

namespace Volo.Abp.DataExporting.Excel;

public class AbpDataExportingExcelTestBase : AbpIntegratedTest<AbpDataExportingExcelTestsModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected async Task OpenExcelFileAsync(ExcelFileDto file)
    {
        var excel = file.FileContent;

        var tmpDir = Path.GetTempPath();
        var name = file.FileName;
        var targetFilename = Path.Combine(tmpDir, name);

        await using var stream = new FileStream(targetFilename, FileMode.Create);
        await stream.WriteAsync(excel, 0, excel.Length);

        Process.Start(new ProcessStartInfo(targetFilename) { UseShellExecute = true });
    }
}