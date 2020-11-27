using System.IO;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Minify.Styles;

namespace Volo.Abp.Cli.Bundling.Styles
{
    public class StyleBundler : BundlerBase, IStyleBundler, ITransientDependency
    {
        public override string FileExtension => ".css";

        public StyleBundler(ICssMinifier minifier)
            : base(minifier)
        {

        }

        public override string GenerateDefinition(string bundleFilePath)
        {
            var lastModifiedTicks = File.GetLastWriteTime(bundleFilePath).Ticks;
            var builder = new StringBuilder();
            builder.AppendLine($"{BundlingConsts.StylePlaceholderStart}");
            builder.AppendLine($"    <link href=\"{Path.GetFileName(bundleFilePath)}?_v={lastModifiedTicks}\" rel=\"stylesheet\"/>");
            builder.Append($"    {BundlingConsts.StylePlaceholderEnd}");
            return builder.ToString();
        }

        protected override string ProcessBeforeAddingToTheBundle(string referencePath, string bundleDirectory, string fileContent)
        {
            return CssRelativePathAdjuster.Adjust(
               fileContent,
               referencePath,
               bundleDirectory
           );
        }
    }
}
