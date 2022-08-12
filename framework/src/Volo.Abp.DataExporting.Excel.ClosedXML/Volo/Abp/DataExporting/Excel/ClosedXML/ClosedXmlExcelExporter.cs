using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using Microsoft.Extensions.Options;
using Volo.Abp.DataExporting.Excel.Core;
using Volo.Abp.DataExporting.Excel.Core.Dto;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.DataExporting.Excel.ClosedXML;

public class ClosedXmlExcelExporter : IExcelExporter, ITransientDependency
{
    public const string EngineName = "ClosedXML";
    public string Name => EngineName;

    private ExcelFileDto _file;
    private XLWorkbook _excel;
    private IXLWorksheet _currentSheet;

    protected AbpDataExportingOptions Options { get; }

    public ClosedXmlExcelExporter(IOptionsMonitor<AbpDataExportingOptions> options)
    {
        Options = options.CurrentValue;
    }

    public void CreateEmptyExcel(string fileName)
    {
        _file = new ExcelFileDto(fileName);
        _excel = new XLWorkbook();
    }

    public void CreateSheet(string name)
    {
        _currentSheet = _excel.Worksheets.Add(name);
    }

    public void SetCurrentSheet(string name)
    {
        _excel.Worksheets.TryGetWorksheet(name, out _currentSheet);
    }

    public void AddHeader(params string[] headerTexts)
    {
        if (headerTexts.IsNullOrEmpty())
        {
            return;
        }

        var row = _currentSheet.FirstRow();
        for (var i = 1; i <= headerTexts.Length; i++)
        {
            row.Cell(i).Value = headerTexts[i-1];
            row.Cell(i).Style.Font.Bold = true;
            row.Cell(i).Style.Fill.PatternType = XLFillPatternValues.Solid;
            row.Cell(i).Style.Fill.BackgroundColor = XLColor.DarkBlue;
            row.Cell(i).Style.Font.FontColor = XLColor.White;
            row.Cell(i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        _currentSheet.RangeUsed().SetAutoFilter();
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
                _currentSheet.Row(i + startRowIndex).Cell(j + 1).Value = propertySelectors[j](items[i]);
            }
        }
    }

    private void SetProperties()
    {
        // set some document properties
        _excel.Properties.Title = Options.Title ?? string.Empty;
        _excel.Properties.Author = Options.Author ?? string.Empty;
        _excel.Properties.Manager = Options.Author ?? string.Empty;
        _excel.Properties.Comments = Options.Comments ?? string.Empty;

        // set some extended property values
        _excel.Properties.Company = Options.Company ?? string.Empty;
    }

    public ExcelFileDto Save()
    {
        SetProperties();
        var stream = _excel.ToMemoryStream();
        _file.FileContent = stream.ToArray();
        return _file;
    }
}