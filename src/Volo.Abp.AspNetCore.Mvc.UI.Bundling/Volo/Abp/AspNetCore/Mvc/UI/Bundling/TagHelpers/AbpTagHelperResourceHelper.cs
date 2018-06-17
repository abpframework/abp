using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public abstract class AbpTagHelperResourceHelper : ITransientDependency
    {
        protected IBundleManager BundleManager { get; }
        protected IHybridWebRootFileProvider WebRootFileProvider { get; }

        protected AbpTagHelperResourceHelper(
            IBundleManager bundleManager, 
            IHybridWebRootFileProvider webRootFileProvider)
        {
            BundleManager = bundleManager;
            WebRootFileProvider = webRootFileProvider;
        }

        public virtual async Task ProcessAsync(
            TagHelperContext context, 
            TagHelperOutput output, 
            string bundleName,
            List<BundleTagHelperItem> files)
        {
            output.TagName = null;

            if (bundleName.IsNullOrEmpty())
            {
                bundleName = GenerateBundleName(files);
            }

            CreateBundle(bundleName, files);

            var bundleFiles = GetBundleFiles(bundleName);

            output.Content.Clear();

            foreach (var bundleFile in bundleFiles)
            {
                var file = WebRootFileProvider.GetFileInfo(bundleFile);
                if (file == null)
                {
                    throw new AbpException($"Could not find the bundle file from {nameof(IHybridWebRootFileProvider)}");
                }

                AddHtmlTag(context, output, bundleFile + "?_v=" + file.LastModified.UtcTicks);
            }
        }

        protected abstract void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems);

        protected abstract IReadOnlyList<string> GetBundleFiles(string bundleName);

        protected abstract void AddHtmlTag(TagHelperContext context, TagHelperOutput output, string file);

        protected virtual string GenerateBundleName(List<BundleTagHelperItem> bundleItems)
        {
            return bundleItems.JoinAsString("|").ToMd5();
        }
    }

    public class AbpTagHelperScriptHelper : AbpTagHelperResourceHelper
    {
        public AbpTagHelperScriptHelper(
            IBundleManager bundleManager, 
            IHybridWebRootFileProvider webRootFileProvider
            ) : base(
            bundleManager, 
            webRootFileProvider)
        {
        }

        protected override void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems)
        {
            BundleManager.CreateScriptBundle(
                bundleName,
                configuration => bundleItems.ForEach(bi => bi.AddToConfiguration(configuration))
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

    public class AbpTagHelperStyleHelper : AbpTagHelperResourceHelper
    {
        public AbpTagHelperStyleHelper(
            IBundleManager bundleManager, 
            IHybridWebRootFileProvider webRootFileProvider
            ) : base(
            bundleManager, 
            webRootFileProvider)
        {
        }

        protected override void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems)
        {
            BundleManager.CreateStyleBundle(
                bundleName,
                configuration => bundleItems.ForEach(bi => bi.AddToConfiguration(configuration))
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