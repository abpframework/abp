using System;
using System.IO;
using NPOI.SS.UserModel;

namespace Volo.Abp.DataExporting.Excel.Npoi;

public static class NpoiExtensions
{


    /// <summary>
    ///  Write the <paramref name="workbook"/> to a memory stream
    /// </summary>
    /// <param name="workbook"></param>
    /// <returns></returns>
    public static MemoryStream ToMemoryStream(this IWorkbook workbook)
    {
        using var output = new MemoryStream();
        workbook.Write(output);
        return output;
    }


    private static ICellStyle _formatDataTime;
    /// <summary>
    /// </summary>
    /// <param name="row"></param>
    /// <param name="workbook"></param>
    /// <param name="columnIndex"></param>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public static void SetCellFormatedValue(this IRow row, IWorkbook workbook, int columnIndex, string type, object value)
    {
        switch (type)
        {
            case "System.Int32":
                row.CreateCell(columnIndex, CellType.Numeric).SetCellValue((int)value);
                break;
            case "System.Double":
                row.CreateCell(columnIndex, CellType.Numeric).SetCellValue((double)value);
                break;
            case "System.Decimal":
                row.CreateCell(columnIndex, CellType.Numeric)
                    .SetCellValue(Convert.ToDouble((decimal)value));
                break;
            case "System.Date":
            case "System.DateTime":
                if (_formatDataTime == null)
                {
                    var dtCellStyle = workbook.CreateCellStyle();

                    dtCellStyle.DataFormat = workbook
                        .GetCreationHelper()
                        .CreateDataFormat()
                        .GetFormat("dd.MM.yyyy H:mm:ss");
                    _formatDataTime = dtCellStyle;
                }

                var rdt = row.CreateCell(columnIndex);
                var dataTime = (DateTime)value;
                if (dataTime.Year < 1900) dataTime = new DateTime(1900, 1, 1);
                rdt.SetCellValue(dataTime);

                rdt.CellStyle = _formatDataTime;
                break;
            case "System.String":
                row.CreateCell(columnIndex, CellType.String).SetCellValue(value.ToString());
                break;
            case "System.Boolean":
                row.CreateCell(columnIndex, CellType.Boolean)
                    .SetCellValue((bool)value ? "Yes" : "No");
                break;
            default:
                row.CreateCell(columnIndex, CellType.String).SetCellValue(value.ToString());
                break;
        }
    }
}