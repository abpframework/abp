using System;
using Volo.Abp.AspNetCore.Mvc.UI.Minification.Scripts;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Scripts
{
    public class ScriptBundler : BundlerBase, IScriptBundler
    {
        public override string FileExtension => "js";

        public ScriptBundler(IWebContentFileProvider webContentFileProvider, IJavascriptMinifier minifier)
            : base(webContentFileProvider, minifier)
        {
        }

        protected override string NormalizedCode(string code)
        {
            return code.EnsureEndsWith(';') + Environment.NewLine;
        }
    }
}