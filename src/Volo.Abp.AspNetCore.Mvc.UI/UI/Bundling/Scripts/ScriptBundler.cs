using Volo.Abp.AspNetCore.Mvc.UI.Minification.Scripts;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Scripts
{
    public class ScriptBundler : BundlerBase, IScriptBundler
    {
        public override string FileExtension => "js";

        public ScriptBundler(IHybridWebRootFileProvider webRootFileProvider, IJavascriptMinifier minifier)
            : base(webRootFileProvider, minifier)
        {
        }
    }
}