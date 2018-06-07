using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Bundling
{
    public class AbpStyleBundleTagHelperService : AbpTagHelperService<AbpStyleBundleTagHelper>
    {
        private readonly IBundleManager _bundleManager;

        public AbpStyleBundleTagHelperService(IBundleManager bundleManager)
        {
            _bundleManager = bundleManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            var bundleName = TagHelper.Name;
            var files = await GetFileList(context, output);
            if (bundleName.IsNullOrEmpty())
            {
                bundleName = GenerateBundleName(context, output, files);
            }

            _bundleManager.CreateDynamicStyleBundle(
                bundleName,
                configuration => configuration.AddFiles(files.ToArray())
            );

            var bundleFiles = _bundleManager.GetStyleBundleFiles(bundleName);
            await output.GetChildContentAsync(); //TODO: Suppress child execution!
            output.Content.Clear();
            AddLinkTags(context, output, bundleFiles);
        }

        protected virtual void AddLinkTags(TagHelperContext context, TagHelperOutput output, List<string> files)
        {
            foreach (var file in files)
            {
                output.Content.AppendHtml(
                    $"<link rel=\"stylesheet\" type=\"text/css\" href=\"{file}\" />{Environment.NewLine}");
            }
        }

        protected virtual string GenerateBundleName(TagHelperContext context, TagHelperOutput output, List<string> fileList)
        {
            return fileList.JoinAsString("|").ToMd5();
        }

        protected virtual async Task<List<string>> GetFileList(TagHelperContext context, TagHelperOutput output)
        {
            var fileList = new List<string>();
            context.Items[AbpBundleFileTagHelperService.ContextFileListKey] = fileList;
            await output.GetChildContentAsync();
            return fileList;
        }
    }
}