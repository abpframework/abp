using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Styles;
using Volo.Abp.Bundling.Styles;
using Volo.Abp.Minify.Styles;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Styles;

public class StyleBundler : MvcUiBundlerBase, IStyleBundler
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    public override string FileExtension => "css";

    public StyleBundler(
        IWebHostEnvironment hostingEnvironment,
        ICssMinifier minifier,
        IOptions<AbpBundlingOptions> bundlingOptions)
        : base(
            hostingEnvironment,
            minifier,
            bundlingOptions)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public string GetAbsolutePath(string relativePath)
    {
        return Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", relativePath.RemovePreFix("/"));
    }

    protected override string ProcessBeforeAddingToTheBundle(IBundlerContext context, string filePath, string fileContent)
    {
        return CssRelativePath.Adjust(
            fileContent,
            GetAbsolutePath(filePath),
            GetAbsolutePath(context.BundleRelativePath)
        );
    }
}
