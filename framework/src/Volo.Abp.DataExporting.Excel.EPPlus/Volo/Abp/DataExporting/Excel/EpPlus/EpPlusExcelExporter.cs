using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Volo.Abp.DataExporting.Excel.Core;
using Volo.Abp.DataExporting.Excel.Core.Dto;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.DataExporting.Excel.EPPlus;

public class EpPlusExcelExporter : IExcelExporter, ITransientDependency
{
    public const string EngineName = "EPPlus";
    public string Name => EngineName;

    private ExcelFileDto _file;
    private ExcelPackage _excel;
    private ExcelWorksheet _currentSheet;

    protected AbpDataExportingOptions Options { get; }

    public EpPlusExcelExporter(IOptionsMonitor<AbpDataExportingOptions> options)
    {
        Options = options.CurrentValue;
    }

    public void CreateEmptyExcel(string fileName)
    {
        _file = new ExcelFileDto(fileName);
        _excel = new ExcelPackage();
    }

    public void CreateSheet(string name)
    {
        _currentSheet = _excel.Workbook.Worksheets.Add(name);
        _currentSheet.OutLineApplyStyle = true;
    }

    public void SetCurrentSheet(string name)
    {
        _currentSheet = _excel.Workbook.Worksheets.First(s=>s.Name == name);
    }

    public void AddHeader(params string[] headerTexts)
    {
        if (headerTexts.IsNullOrEmpty())
        {
            return;
        }

        for (var i = 0; i < headerTexts.Length; i++)
        {
            AddHeader(_currentSheet, i + 1, headerTexts[i]);
        }
    }

    protected void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
    {
        var cell = sheet.Cells[1, columnIndex];
        cell.Style.Font.Bold = true;
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        cell.Style.Font.Color.SetColor(Color.White);
        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        cell.Value = headerText;
        //cell.AutoSizeColumn(column.Ordinal);
    }

    protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IEnumerable<T> items, params Func<T, object>[] propertySelectors)
    {
        var arrayItems = items.ToArray();
        if (!arrayItems.Any() || propertySelectors.IsNullOrEmpty())
        {
            return;
        }

        for (var i = 0; i < arrayItems.Length; i++)
        {
            for (var j = 0; j < propertySelectors.Length; j++)
            {
                sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](arrayItems[i]);
            }
        }
    }

    public void AddObjects<T>(int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
    {
        if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
        {
            return;
        }

        for (var i = 0; i < items.Count; i++)
        {
            for (var j = 0; j < propertySelectors.Length; j++)
            {
                _currentSheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
            }
        }
    }
    
    private void SetProperties()
    {
        // set some document properties
        _excel.Workbook.Properties.Title = Options.Title;
        _excel.Workbook.Properties.Author = Options.Author;
        _excel.Workbook.Properties.Comments = Options.Comments;

        // set some extended property values
        _excel.Workbook.Properties.Company = Options.Company;

    }

    public ExcelFileDto Save()
    {
        //(Optional) freeze the header row so it is not scrolled
        //_currentSheet.View.FreezePanes(0,headerTexts.Length-1);

        //Create an autofilter for the range
        _currentSheet.Cells[_currentSheet.Dimension.Address].AutoFilter = true;

        _currentSheet.Cells.AutoFitColumns();

        SetProperties();
        _file.FileContent = _excel.GetAsByteArray();
        return _file;
    }
}