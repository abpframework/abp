using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.DataExporting.Excel.Core.Dto;

public class ExcelFileDto
{
    public const string ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string FileExtension = ".xlsx";

    [Required]
    public string FileName { get; set; }

    [Required] public string FileType => ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet;

    [Required]
    public byte[] FileContent { get; set; }

    private ExcelFileDto()
    {

    }

    public ExcelFileDto(string fileName)
    {
        if (!fileName.EndsWith(FileExtension))
        {
            fileName += FileExtension;
        }

        FileName = fileName;
    }
}