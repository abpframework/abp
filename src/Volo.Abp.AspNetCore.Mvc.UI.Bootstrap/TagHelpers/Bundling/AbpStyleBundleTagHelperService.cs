using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Bundling
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

        protected override List<string> GetBundleFiles(string bundleName)
        {
            return BundleManager.GetStyleBundleFiles(bundleName);
        }

        protected override void AddHtmlTags(TagHelperContext context, TagHelperOutput output, List<string> files)
        {
            foreach (var file in files)
            {
                output.Content.AppendHtml(
                    $"<link rel=\"stylesheet\" type=\"text/css\" href=\"{file}\" />{Environment.NewLine}");
            }
        }
    }
}