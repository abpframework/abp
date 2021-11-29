using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Volo.Abp.Bundling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Minify.Scripts;

namespace Volo.Abp.Cli.Bundling.Scripts
{
    public class ScriptBundler : BundlerBase, IScriptBundler, ITransientDependency
    {
        public override string FileExtension => ".js";

        public ScriptBundler(IJavascriptMinifier minifier)
            : base(minifier)
        {
        }

        public override string GenerateDefinition(string bundleFilePath,
            List<BundleDefinition> bundleDefinitionsExcludingFromBundle)
        {
            var lastModifiedTicks = File.GetLastWriteTime(bundleFilePath).Ticks;
            var builder = new StringBuilder();
            builder.AppendLine($"{BundlingConsts.ScriptPlaceholderStart}");
            builder.AppendLine(
                $"    <script src=\"{Path.GetFileName(bundleFilePath)}?_v={lastModifiedTicks}\"></script>");

            foreach (var bundleDefinition in bundleDefinitionsExcludingFromBundle)
            {
                builder.Append($"    <script src=\"{bundleDefinition.Source}\"");
                foreach (var additionalProperty in bundleDefinition.AdditionalProperties)
                {
                    builder.Append($" {additionalProperty.Key}={additionalProperty.Value} ");
                }

                builder.AppendLine("></script>");
            }
            
            builder.Append($"    {BundlingConsts.ScriptPlaceholderEnd}");
            return builder.ToString();
        }

        protected override string ProcessBeforeAddingToTheBundle(string referencePath, string bundleDirectory,
            string fileContent)
        {
            return fileContent.EnsureEndsWith(';') + Environment.NewLine;
        }
    }
}