using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.DataExporting.Excel.Core;
using Volo.Abp.DataExporting.Excel.EPPlus;
using Xunit;

namespace Volo.Abp.DataExporting.Excel;

public class EpPlusExcelExporter_Tests : AbpDataExportingExcelTestBase
{
    private readonly IExcelExporter _excelExporter;

    public EpPlusExcelExporter_Tests()
    {
        var excelExporters = GetRequiredService<IEnumerable<IExcelExporter>>();
        _excelExporter = excelExporters.FirstOrDefault(c => c.Name.Equals(EpPlusExcelExporter.EngineName));
    }

    [Fact]
    public async Task Export_Excel_EpPlus_Test()
    {
        if (_excelExporter is not null)
        {
            //Arrange
            _excelExporter.CreateEmptyExcel("sample.xlsx");
            _excelExporter.CreateSheet("Test");
            _excelExporter.AddHeader("Id", "FirstName", "LastName", "BirthDate", "Col1", "Col2", "Col3", "Col4");
            _excelExporter.AddObjects(2,
                new List<TestExportModelDto>
                {
                    new() { Id = "1", FirstName = "Iulian", LastName = "Alexe", BirthDate = new DateTime(2000, 08, 12), Col1 = "col1 y data test", Col2 = "col2 y data test", Col3 = "col3 y data test", Col4 = "col4 x data test" },
                    new() { Id = "2", FirstName = "George", LastName = "Vlad", BirthDate = new DateTime(2000, 03, 30), Col1 = "col1 x data test", Col2 = "col2 x data test", Col3 = "col3 x data test", Col4 = "col4 x data test" },
                },
                _ => _.Id,
                _ => _.FirstName,
                _ => _.LastName,
                _ => _.BirthDate.ToShortDateString(),
                _ => _.Col1,
                _ => _.Col2,
                _ => _.Col3,
                _ => _.Col4
            );

            //Act
            var result = _excelExporter.Save();

            //Assert
            result.ShouldNotBeNull();

            await OpenExcelFileAsync(result);
        }
        else
        {
            Assert.True(true, "Not registered module");
        }
    }
}