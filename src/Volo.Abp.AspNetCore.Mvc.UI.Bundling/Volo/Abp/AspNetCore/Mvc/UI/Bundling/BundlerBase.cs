using System.Text;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.AspNetCore.Mvc.UI.Minification;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public abstract class BundlerBase : IBundler, ITransientDependency
    {
        protected IHybridWebRootFileProvider WebRootFileProvider { get; }
        protected IMinifier Minifier { get; }

        protected BundlerBase(IHybridWebRootFileProvider webRootFileProvider, IMinifier minifier)
        {
            WebRootFileProvider = webRootFileProvider;
            Minifier = minifier;
        }

        public abstract string FileExtension { get; }

        public BundleResult Bundle(IBundlerContext context)
        {
            var sb = new StringBuilder();

            foreach (var file in context.ContentFiles)
            {
                sb.AppendLine(GetFileContent(context, file));
            }

            var bundleContent = sb.ToString();

            if (context.IsMinificationEnabled)
            {
                bundleContent = Minifier.Minify(bundleContent, context.BundleRelativePath);
            }

            return new BundleResult(bundleContent);
        }

        protected virtual string GetFileContent(IBundlerContext context, string file)
        {
            return GetFileInfo(context, file).ReadAsString();
        }

        protected virtual IFileInfo GetFileInfo(IBundlerContext context, string file)
        {
            var fileInfo = WebRootFileProvider.GetFileInfo(file);

            if (!fileInfo.Exists)
            {
                throw new AbpException($"Could not find file '{file}' using {nameof(IHybridWebRootFileProvider)}");
            }

            return fileInfo;
        }
    }
}