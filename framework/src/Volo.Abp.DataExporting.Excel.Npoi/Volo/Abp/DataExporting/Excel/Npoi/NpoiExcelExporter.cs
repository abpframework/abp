using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NPOI.HSSF.Util;
using Volo.Abp.DataExporting.Excel.Core;
using Volo.Abp.DependencyInjection;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Volo.Abp.DataExporting.Excel.Core.Dto;

namespace Volo.Abp.DataExporting.Excel.Npoi;

public class NpoiExcelExporter : IExcelExporter, ITransientDependency
{
    public const string EngineName = "NPOI";
    public string Name => EngineName;

    private ExcelFileDto _file;
    private XSSFWorkbook _excel;
    private ISheet _currentSheet;
    private ICellStyle _dateCellStyle;

    private int _totalColumn  = int.MaxValue;
    protected AbpDataExportingOptions Options { get; }

    public NpoiExcelExporter(IOptionsMonitor<AbpDataExportingOptions> options)
    {
        Options = options.CurrentValue;
    }

    public void CreateEmptyExcel(string fileName)
    {
        _file = new ExcelFileDto(fileName);
        _excel = new XSSFWorkbook();
    }

    public void CreateSheet(string name)
    {
        _currentSheet = _excel.CreateSheet(name);
    }
    public void SetCurrentSheet(string name)
    {
        _currentSheet = _excel.GetSheet(name);
    }

    //private void CreateEmptySheet()
    //{
    //    _currentSheet = _excel.CreateSheet();
    //}

    public void AddHeader(params string[] headerTexts)
    {
        if (headerTexts.IsNullOrEmpty())
        {
            return;
        }

        _currentSheet.CreateRow(0);

        for (var i = 0; i < headerTexts.Length; i++)
        {
            AddHeader(i, headerTexts[i]);
        }

        _totalColumn = headerTexts.Length - 1;
    }

    protected void AddHeader(int columnIndex, string headerText)
    {
        var cell = _currentSheet.GetRow(0).CreateCell(columnIndex);
        
        var cellStyle = _currentSheet.Workbook.CreateCellStyle();
        cellStyle.FillForegroundColor = HSSFColor.DarkBlue.Index;
        cellStyle.FillPattern = FillPattern.SolidForeground;

        var font = _currentSheet.Workbook.CreateFont();
        font.IsBold = true;
        font.FontHeightInPoints = 12;
        font.Color = HSSFColor.White.Index;

        cellStyle.SetFont(font);
        cell.CellStyle = cellStyle;

        cell.SetCellValue(headerText);

        _currentSheet.AutoSizeColumn(columnIndex);
    }

    protected void SetAutoFilter()
    {
        const int firstRow = 0;
        var lastRow = _currentSheet.LastRowNum;
        const int firstCol = 0;
        var lastCol = _totalColumn;
        _currentSheet.SetAutoFilter(new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
    }

    public void AddObjects<T>(int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
    {
        if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            return;

        for (var i = 1; i <= items.Count; i++)
        {
            var row = _currentSheet.CreateRow(i);

            for (var j = 0; j < propertySelectors.Length; j++)
            {

                var type = items[i - 1];
                var value = propertySelectors[j](type);
                if (value != null)
                {
                    row.SetCellFormatedValue(_currentSheet.Workbook, j, type.GetType().ToString(), value);
                }
            }
        }

        //NpoiExtensions.FormatDataTime = null; //fix error: This Style does not belong to the supplied Workbook. Are you trying to assign a style from one workbook to the cell of a differnt workbook?
    }

    public ExcelFileDto Save()
    {
        SetAutoFilter();

        SetProperties();
        var stream = _excel.ToMemoryStream();
        _file.FileContent = stream.ToArray();
        return _file;
    }

    private ICellStyle GetDateCellStyle(ICell cell)
    {
        return _dateCellStyle ??= cell.Sheet.Workbook.CreateCellStyle();
    }

    protected void SetCellDataFormat(ICell cell, string dataFormat)
    {
        if (cell == null)
        {
            return;
        }

        var dateStyle = GetDateCellStyle(cell);
        var format = cell.Sheet.Workbook.CreateDataFormat();
        dateStyle.DataFormat = format.GetFormat(dataFormat);
        cell.CellStyle = dateStyle;
        if (DateTime.TryParse(cell.StringCellValue, out var datetime))
        {
            cell.SetCellValue(datetime);
        }
    }

    private void SetProperties()
    {
        var xmlProps = _excel.GetProperties();
        var coreProps = xmlProps.CoreProperties;

        // set some document properties
        coreProps.Title = Options.Title ?? string.Empty;
        coreProps.Creator = Options.Company ?? string.Empty;
        coreProps.Description = Options.Comments ?? string.Empty; 

    }
}