using System;
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

        protected IWebContentFileProvider WebContentFileProvider { get; }
        protected IMinifier Minifier { get; }

        protected BundlerBase(IWebContentFileProvider webContentFileProvider, IMinifier minifier)
        {
            WebContentFileProvider = webContentFileProvider;
            Minifier = minifier;

            Logger = NullLogger<BundlerBase>.Instance;
        }

        public abstract string FileExtension { get; }

        public BundleResult Bundle(IBundlerContext context)
        {
            Logger.LogInformation($"Bundling {context.BundleRelativePath} ({context.ContentFiles.Count} files)");

            var bundleContentBuilder = new StringBuilder();

            Logger.LogDebug("Bundle files:");
            foreach (var file in context.ContentFiles)
            {
                AddFileToBundle(context, bundleContentBuilder, file);
            }

            var bundleContent = bundleContentBuilder.ToString();
            Logger.LogInformation($"Bundled {context.BundleRelativePath} ({bundleContent.Length} bytes)");

            return new BundleResult(bundleContent);
        }

        private void AddFileToBundle(IBundlerContext context, StringBuilder bundleContentBuilder, string fileName)
        {
            string fileContent = null;

            if (context.IsMinificationEnabled && !IsMinFile(fileName))
            {
                var minFileInfo = GetMinFileInfoOrNull(fileName);
                if (minFileInfo != null)
                {
                    Logger.LogDebug($"- {fileName} ({minFileInfo.Length} bytes) - already minified");
                    fileContent = minFileInfo.ReadAsString();
                }
            }

            if (fileContent == null)
            {
                fileContent = GetFileContent(context, fileName);
                Logger.LogDebug($"- {fileName} ({fileContent.Length} bytes) - non minified");

                if (context.IsMinificationEnabled)
                {
                    var nonMinifiedSize = fileContent.Length;
                    fileContent = Minifier.Minify(fileContent, context.BundleRelativePath);
                    Logger.LogInformation($"  > Minified {fileName} ({nonMinifiedSize} bytes -> {fileContent.Length} bytes)");
                }
            }

            fileContent = ProcessBeforeAddingToTheBundle(context, fileName, fileContent);
            bundleContentBuilder.Append(fileContent);
        }

        protected virtual string GetFileContent(IBundlerContext context, string file)
        {
            return GetFileInfo(context, file).ReadAsString();
        }

        protected virtual IFileInfo GetFileInfo(IBundlerContext context, string file)
        {
            var fileInfo = WebContentFileProvider.GetFileInfo(file);

            if (!fileInfo.Exists)
            {
                throw new AbpException($"Could not find file '{file}' using {nameof(IWebContentFileProvider)}");
            }

            return fileInfo;
        }

        protected virtual bool IsMinFile(string fileName)
        {
            return fileName.EndsWith($".min.{FileExtension}", StringComparison.InvariantCultureIgnoreCase);
        }

        protected virtual IFileInfo GetMinFileInfoOrNull(string file)
        {
            var fileInfo = WebContentFileProvider.GetFileInfo($"{file.RemovePostFix($".{FileExtension}")}.min.{FileExtension}");

            return fileInfo.Exists ? fileInfo : null;
        }

        protected virtual string ProcessBeforeAddingToTheBundle(IBundlerContext context, string filePath, string fileContent)
        {
            return fileContent;
        }
    }
}