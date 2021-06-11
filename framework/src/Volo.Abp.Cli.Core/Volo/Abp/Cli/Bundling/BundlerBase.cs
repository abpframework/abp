using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Volo.Abp.Bundling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Minify;
using Volo.Abp.Minify.NUglify;

namespace Volo.Abp.Cli.Bundling
{
    public abstract class BundlerBase : IBundler, ITransientDependency
    {
        private static string[] _minFileSuffixes = {"min", "prod"};

        protected IMinifier Minifier { get; }
        public ILogger<BundlerBase> Logger { get; set; }
        public abstract string FileExtension { get; }
        public abstract string GenerateDefinition(string bundleFilePath,
            List<BundleDefinition> bundleDefinitionsExcludingFromBundle);

        protected BundlerBase(IMinifier minifier)
        {
            Minifier = minifier;
        }

        public string Bundle(BundleOptions options, BundleContext context)
        {
            var bundleFilePath = Path.Combine(PathHelper.GetWwwRootPath(options.Directory),
                $"{options.BundleName}{FileExtension}");
            var bundleFileDefinitions = context.BundleDefinitions.Where(t => t.ExcludeFromBundle == false).ToList();
            var fileDefinitionsExcludingFromBundle = context.BundleDefinitions.Where(t => t.ExcludeFromBundle).ToList();
            
            var bundledContent = BundleFiles(options, bundleFileDefinitions);
            File.WriteAllText(bundleFilePath, bundledContent);

            return GenerateDefinition(bundleFilePath,fileDefinitionsExcludingFromBundle);
        }

        private bool IsMinFile(string fileName)
        {
            foreach (var suffix in _minFileSuffixes)
            {
                if (fileName.EndsWith($".{suffix}{FileExtension}", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private string BundleFiles(BundleOptions options, List<BundleDefinition> bundleDefinitions)
        {
            var staticAssetsFilePath = Path.Combine(options.Directory, "bin", "Debug", options.FrameworkVersion,
                $"{options.ProjectFileName}.StaticWebAssets.xml");
            if (!File.Exists(staticAssetsFilePath))
            {
                throw new BundlingException(
                    "Unable to find static web assets file. You need to build the project to generate static web assets file.");
            }

            var staticAssetsDefinitions = new XmlDocument();
            staticAssetsDefinitions.Load(staticAssetsFilePath);

            var builder = new StringBuilder();
            foreach (var definition in bundleDefinitions)
            {
                string content;
                if (definition.Source.StartsWith("_content"))
                {
                    var pathFragments = definition.Source.Split('/').ToList();
                    var basePath = $"{pathFragments[0]}/{pathFragments[1]}";
                    var node = staticAssetsDefinitions.SelectSingleNode($"//ContentRoot[@BasePath='{basePath}']");
                    if (node?.Attributes == null)
                    {
                        throw new AbpException("Not found: " + definition.Source);
                    }
                    
                    var path = node.Attributes["Path"].Value;
                    var absolutePath = definition.Source.Replace(basePath, path);
                    content = GetFileContent(absolutePath, options.Minify);
                }
                else if (definition.Source.StartsWith("_framework"))
                {
                    var slashIndex = definition.Source.IndexOf('/');
                    var fileName =
                        definition.Source.Substring(slashIndex + 1, definition.Source.Length - slashIndex - 1);
                    var filePath =
                        Path.Combine(PathHelper.GetFrameworkFolderPath(options.Directory, options.FrameworkVersion),
                            fileName);
                    content = GetFileContent(filePath, false);
                }
                else
                {
                    var filePath = Path.Combine(PathHelper.GetWwwRootPath(options.Directory), definition.Source);
                    content = GetFileContent(filePath, options.Minify);
                }

                content = ProcessBeforeAddingToTheBundle(definition.Source, Path.Combine(options.Directory, "wwwroot"),
                    content);
                builder.AppendLine(content);
            }

            return builder.ToString();
        }

        private string GetFileContent(string filePath, bool minify)
        {
            var content = File.ReadAllText(filePath);
            if (minify && !IsMinFile(filePath))
            {
                try
                {
                    content = Minifier.Minify(content);
                }
                catch (NUglifyException ex)
                {
                    Logger.LogWarning(
                        $"Unable to minify the file: {Path.GetFileName(filePath)}. Adding file to the bundle without minification.",
                        ex);
                }
            }

            return content;
        }

        protected virtual string ProcessBeforeAddingToTheBundle(string referencePath, string bundleDirectory,
            string fileContent)
        {
            return fileContent;
        }
    }
}