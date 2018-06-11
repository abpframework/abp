using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Bundling
{
    public abstract class AbpBundleTagHelperServiceBase<TTagHelper> : AbpTagHelperService<TTagHelper>
        where TTagHelper : TagHelper, IBundleTagHelper
    {
        protected IBundleManager BundleManager { get; }

        protected AbpBundleTagHelperServiceBase(IBundleManager bundleManager)
        {
            BundleManager = bundleManager;
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

            CreateBundle(bundleName, files);

            var bundleFiles = GetBundleFiles(bundleName);

            output.Content.Clear();
            AddHtmlTags(context, output, bundleFiles);
        }

        protected abstract void CreateBundle(string bundleName, List<string> files);

        protected abstract List<string> GetBundleFiles(string bundleName);

        protected abstract void AddHtmlTags(TagHelperContext context, TagHelperOutput output, List<string> files);

        protected virtual string GenerateBundleName(TagHelperContext context, TagHelperOutput output, List<string> fileList)
        {
            return fileList.JoinAsString("|").ToMd5();
        }

        protected virtual async Task<List<string>> GetFileList(TagHelperContext context, TagHelperOutput output)
        {
            var fileList = new List<string>();
            context.Items[AbpBundleFileTagHelperService.ContextFileListKey] = fileList;
            await output.GetChildContentAsync(); //TODO: Suppress child execution!
            return fileList;
        }
    }
}