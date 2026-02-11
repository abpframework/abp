using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.Minify;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public abstract class MvcUiBundlerBase : BundlerBase
{
    protected IWebHostEnvironment WebHostingEnvironment { get; }

    protected MvcUiBundlerBase(
        IWebHostEnvironment webHostingEnvironment,
        IMinifier minifier,
        IOptions<AbpBundlingOptions> bundlingOptions) : base(minifier, bundlingOptions)
    {
        WebHostingEnvironment = webHostingEnvironment;
    }

    protected override IFileInfo FindFileInfo(string file)
    {
        return WebHostingEnvironment.WebRootFileProvider.GetFileInfo(file);
    }
}