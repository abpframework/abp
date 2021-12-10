namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleResult
{
    public string Content { get; }

    public BundleResult(string content)
    {
        Content = content;
    }
}
