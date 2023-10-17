namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleFile
{
    public string File { get; set; }

    public bool IsCdn { get; set; }

    public BundleFile(string file, bool isCdn = false)
    {
        File = file;
        IsCdn = isCdn;
    }
}
