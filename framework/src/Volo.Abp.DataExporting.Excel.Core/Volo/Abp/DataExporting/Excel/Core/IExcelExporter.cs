using System;
using System.Collections.Generic;
using Volo.Abp.DataExporting.Excel.Core.Dto;

namespace Volo.Abp.DataExporting.Excel.Core;

public interface IExcelExporter
{
    public string Name { get; }

    void CreateEmptyExcel(string fileName);
    void CreateSheet(string name);
    void SetCurrentSheet(string name);

    void AddHeader(params string[] headerTexts);

    void AddObjects<T>(int startRowIndex, IList<T> items,
        params Func<T, object>[] propertySelectors);

    ExcelFileDto Save();
}