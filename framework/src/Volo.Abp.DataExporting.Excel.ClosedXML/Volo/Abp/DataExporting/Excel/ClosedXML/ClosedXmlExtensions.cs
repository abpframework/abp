using System.IO;
using ClosedXML.Excel;

namespace Volo.Abp.DataExporting.Excel.ClosedXML;

public static class ClosedXmlExtensions
{
    /// <summary>
    ///  Write the <paramref name="workbook"/> to a memory stream
    /// </summary>
    /// <param name="workbook"></param>
    /// <returns></returns>
    public static MemoryStream ToMemoryStream(this XLWorkbook workbook)
    {
        using var output = new MemoryStream();
        workbook.SaveAs(output);
        return output;
    }

}