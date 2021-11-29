namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public interface IBundler
{
    string FileExtension { get; }

    BundleResult Bundle(IBundlerContext context);
}
