using System.Collections.Generic;

namespace Volo.Abp.DataExporting.Excel.Core;

public class AbpDataExportingOptions
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Comments { get; set; }
    public string Company { get; set; }
    public string CheckedBy { get; set; }
    public Dictionary<string,string> OtherProperties { get; set; }

    public AbpDataExportingOptions()
    {
        OtherProperties = new Dictionary<string, string>();
    }
}