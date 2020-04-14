using System;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Minify;

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
            var fileContent = GetFileContentConsideringMinification(context, fileName);
            fileContent = ProcessBeforeAddingToTheBundle(context, fileName, fileContent);
            bundleContentBuilder.Append(fileContent);
        }

        private string GetFileContentConsideringMinification(IBundlerContext context, string fileName)
        {
            var isMinFile = IsMinFile(fileName);
            if (!context.IsMinificationEnabled || isMinFile)
            {
                var fileContent = GetFileInfo(context, fileName).ReadAsString();
                Logger.LogDebug($"- {fileName} ({fileContent.Length} bytes)");
                if (context.IsMinificationEnabled && isMinFile)
                {
                    Logger.LogDebug("  > Already minified");
                }

                return fileContent;
            }

            var minFileInfo = GetMinFileInfoOrNull(fileName);
            if (minFileInfo != null)
            {
                var fileContent = minFileInfo.ReadAsString();
                Logger.LogDebug($"- {fileName}");
                Logger.LogDebug($"  > Using the pre-minified file: {minFileInfo.Name} ({fileContent.Length} bytes)");
                return fileContent;
            }

            return GetAndMinifyFileContent(context, fileName);
        }

        private string GetAndMinifyFileContent(IBundlerContext context, string fileName)
        {
            var fileContent = GetFileInfo(context, fileName).ReadAsString();
            var nonMinifiedSize = fileContent.Length;
            
            Logger.LogDebug($"- {fileName} ({nonMinifiedSize} bytes) - non minified, minifying...");

            fileContent = Minifier.Minify(
                fileContent,
                context.BundleRelativePath,
                fileName
            );

            Logger.LogInformation($"  > Minified {fileName} ({nonMinifiedSize} bytes -> {fileContent.Length} bytes)");

            return fileContent;
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