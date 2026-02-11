using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Bundling;

public interface IBundler
{
    string FileExtension { get; }

    BundleResult Bundle(IBundlerContext context);
}
