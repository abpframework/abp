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
            return CssRelativePath.Adjust(
                base.GetFileContent(context, file),
                WebRootFileProvider.GetAbsolutePath(file),
                WebRootFileProvider.GetAbsolutePath(context.BundleRelativePath)
            );
        }
    }
}