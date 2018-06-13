using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpStyleBundleTagHelperService : AbpBundleTagHelperServiceBase<AbpStyleBundleTagHelper>
    {
        public AbpStyleBundleTagHelperService(IBundleManager bundleManager)
        : base(bundleManager)
        {

        }

        protected override void CreateBundle(string bundleName, List<string> files)
        {
            BundleManager.CreateStyleBundle(
                bundleName,
                configuration => configuration.AddFiles(files.ToArray())
            );
        }

        protected override IReadOnlyList<string> GetBundleFiles(string bundleName)
        {
            return BundleManager.GetStyleBundleFiles(bundleName);
        }

        protected override void AddHtmlTag(TagHelperContext context, TagHelperOutput output, string file)
        {
            output.Content.AppendHtml($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{file}\" />{Environment.NewLine}");
        }
    }
}