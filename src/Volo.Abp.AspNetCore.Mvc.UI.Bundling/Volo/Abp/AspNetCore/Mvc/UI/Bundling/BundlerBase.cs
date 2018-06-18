using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Mvc.UI.Minification;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public abstract class BundlerBase : IBundler, ITransientDependency
    {
        public ILogger<BundlerBase> Logger { get; set; }

        protected IHybridWebRootFileProvider WebRootFileProvider { get; }
        protected IMinifier Minifier { get; }

        protected BundlerBase(IHybridWebRootFileProvider webRootFileProvider, IMinifier minifier)
        {
            WebRootFileProvider = webRootFileProvider;
            Minifier = minifier;

            Logger = NullLogger<BundlerBase>.Instance;
        }

        public abstract string FileExtension { get; }

        public BundleResult Bundle(IBundlerContext context)
        {
            Logger.LogDebug($"Bundling {context.BundleRelativePath}");

            var sb = new StringBuilder();

            foreach (var file in context.ContentFiles)
            {
                sb.AppendLine(GetFileContent(context, file));
            }

            var bundleContent = sb.ToString();

            if (context.IsMinificationEnabled)
            {
                Logger.LogDebug($"Minifying {context.BundleRelativePath}");
                bundleContent = Minifier.Minify(bundleContent, context.BundleRelativePath);
            }

            Logger.LogDebug($"Bundled {context.BundleRelativePath}");

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