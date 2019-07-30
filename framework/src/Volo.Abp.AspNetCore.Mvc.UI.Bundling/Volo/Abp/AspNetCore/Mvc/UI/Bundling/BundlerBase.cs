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

            var sb = new StringBuilder();

            Logger.LogDebug("Bundle files:");
            foreach (var file in context.ContentFiles)
            {
                var fileInfo = GetFileInfo(context, file);
                var fileContent = fileInfo.ReadAsString();

                Logger.LogDebug($"- {file} ({fileContent.Length} bytes)");

                if (IsMinFile(fileInfo))
                {
                    sb.Append(NormalizedCode(fileContent));
                }
                else
                {
                    var minFileContent = GetMinFileOrNull(file);
                    if (minFileContent != null)
                    {
                        sb.Append(NormalizedCode(minFileContent));
                    }
                    else
                    {
                        if (context.IsMinificationEnabled)
                        {
                            Logger.LogInformation($"Minifying {context.BundleRelativePath} ({fileInfo.Length} bytes)");
                            sb.Append(NormalizedCode(Minifier.Minify(fileContent, context.BundleRelativePath)));
                        }
                        else
                        {
                            sb.Append(NormalizedCode(fileContent));
                        }
                    }
                }
            }

            var bundleContent = sb.ToString();
            Logger.LogInformation($"Bundled {context.BundleRelativePath} ({bundleContent.Length} bytes)");

            return new BundleResult(bundleContent);
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

        protected virtual bool IsMinFile(IFileInfo fileInfo)
        {
            return fileInfo.Name.EndsWith($".min.{FileExtension}", StringComparison.InvariantCultureIgnoreCase);
        }

        protected virtual string GetMinFileOrNull(string file)
        {
            var fileInfo =
                WebContentFileProvider.GetFileInfo($"{file.RemovePostFix($".{FileExtension}")}.min.{FileExtension}");

            return fileInfo.Exists ? fileInfo.ReadAsString() : null;
        }

        protected virtual string NormalizedCode(string code)
        {
            return code;
        }
    }
}