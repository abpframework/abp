using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleFile
{
    public string FileName { get; set; }

    public bool IsExternalFile { get; set; }

    public BundleFile(string fileName)
    {
        FileName = fileName;
        IsExternalFile = fileName.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                fileName.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }

    public BundleFile(string fileName, bool isExternalFile)
    {
        FileName = fileName;
        IsExternalFile = isExternalFile;
    }

    /// <summary>
    /// This method is used to compatible with old code.
    /// </summary>
    public static implicit operator BundleFile(string fileName)
    {
        return new BundleFile(fileName);
    }
}
