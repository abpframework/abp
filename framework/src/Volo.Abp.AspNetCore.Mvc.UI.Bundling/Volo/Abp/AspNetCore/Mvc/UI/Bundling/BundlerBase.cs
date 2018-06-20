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
            Logger.LogInformation($"Bundling {context.BundleRelativePath} ({context.ContentFiles.Count} files)");

            var sb = new StringBuilder();

            Logger.LogDebug("Bundle files:");
            foreach (var file in context.ContentFiles)
            {
                var fileContent = GetFileContent(context, file);
                Logger.LogDebug($"- {file} ({fileContent.Length} bytes)");
                sb.AppendLine(fileContent);
            }

            var bundleContent = sb.ToString();

            if (context.IsMinificationEnabled)
            {
                Logger.LogInformation($"Minifying {context.BundleRelativePath} ({bundleContent.Length} bytes)");
                bundleContent = Minifier.Minify(bundleContent, context.BundleRelativePath);
            }

            Logger.LogInformation($"Bundled {context.BundleRelativePath} ({bundleContent.Length} bytes)");

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