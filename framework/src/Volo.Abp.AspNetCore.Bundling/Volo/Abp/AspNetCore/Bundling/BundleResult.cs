namespace Volo.Abp.AspNetCore.Bundling;

public class BundleResult
{
    public string Content { get; }

    public BundleResult(string content)
    {
        Content = content;
    }
}
