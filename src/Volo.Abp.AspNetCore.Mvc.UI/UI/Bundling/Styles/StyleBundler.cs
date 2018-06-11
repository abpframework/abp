using System;
using Volo.Abp.AspNetCore.Mvc.UI.Minification.Styles;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Styles
{
    public class StyleBundler : BundlerBase, IStyleBundler
    {
        public override string FileExtension => "css";

        public StyleBundler(IHybridWebRootFileProvider webRootFileProvider, ICssMinifier minifier) 
            : base(webRootFileProvider, minifier)
        {

        }

        protected override string GetFileContent(IBundlerContext context, string file)
        {
            var content = base.GetFileContent(context, file);
            return CssRelativePath.Adjust(
                content,
                WebRootFileProvider.GetAbsolutePath(file),
                WebRootFileProvider.GetAbsolutePath(context.BundleRelativePath)
            );
        }
    }
}