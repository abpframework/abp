using System;

namespace Volo.Abp.VirtualFileExplorer.Web.Models;

public class FileInfoViewModel
{
    public string FilePath { get; set; }

    public string Icon { get; set; }

    public string FileType { get; set; }

    public string Length { get; set; }

    public string FileName { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public bool IsDirectory { get; set; }
}
