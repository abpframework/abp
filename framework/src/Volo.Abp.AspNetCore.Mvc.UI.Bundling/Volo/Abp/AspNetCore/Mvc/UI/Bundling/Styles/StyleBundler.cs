using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.AspNetCore.Mvc.UI.Minification.Styles;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Styles
{
    public class StyleBundler : BundlerBase, IStyleBundler
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public override string FileExtension => "css";

        public StyleBundler(IWebContentFileProvider webContentFileProvider, ICssMinifier minifier, IHostingEnvironment hostingEnvironment) 
            : base(webContentFileProvider, minifier)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        protected override string GetFileContent(IBundlerContext context, string file)
        {
            return CssRelativePath.Adjust(
                base.GetFileContent(context, file),
                GetAbsolutePath(file),
                GetAbsolutePath(context.BundleRelativePath)
            );
        }

        public string GetAbsolutePath(string relativePath)
        {
            return Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", relativePath.RemovePreFix("/"));
        }
    }
}