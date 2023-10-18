namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleFile
{
    public string FileName { get; set; }

    public bool IsCdn { get; set; }

    public BundleFile(string fileName, bool isCdn = false)
    {
        FileName = fileName;
        IsCdn = isCdn;
    }

    /// <summary>
    /// This method is used to compatible with old code.
    /// </summary>
    public static implicit operator BundleFile(string fileName)
    {
        return new BundleFile(fileName);
    }
}
