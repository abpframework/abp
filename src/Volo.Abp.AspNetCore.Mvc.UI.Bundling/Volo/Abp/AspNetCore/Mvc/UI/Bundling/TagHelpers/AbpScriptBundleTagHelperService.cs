using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpScriptBundleTagHelperService : AbpBundleTagHelperServiceBase<AbpScriptBundleTagHelper>
    {
        public AbpScriptBundleTagHelperService(
            IBundleManager bundleManager,
            IHybridWebRootFileProvider webRootFileProvider)
            : base(
                bundleManager, 
                webRootFileProvider)
        {

        }

        protected override void CreateBundle(string bundleName, List<string> files)
        {
            BundleManager.CreateScriptBundle(
                bundleName,
                configuration => configuration.AddFiles(files.ToArray())
            );
        }

        protected override IReadOnlyList<string> GetBundleFiles(string bundleName)
        {
            return BundleManager.GetScriptBundleFiles(bundleName);
        }

        protected override void AddHtmlTag(TagHelperContext context, TagHelperOutput output, string file)
        {
            output.Content.AppendHtml($"<script src=\"{file}\" type=\"text/javascript\"></script>{Environment.NewLine}");
        }
    }
}