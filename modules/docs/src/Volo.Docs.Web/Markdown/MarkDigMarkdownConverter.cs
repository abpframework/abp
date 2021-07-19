using System.Text;
using Markdig;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Markdown.Extensions;

namespace Volo.Docs.Markdown
{
    public class MarkDigMarkdownConverter : IMarkdownConverter, ISingletonDependency
    {
        readonly MarkdownPipeline _markdownPipeline;

        public MarkDigMarkdownConverter()
        {
            _markdownPipeline = new MarkdownPipelineBuilder()
              .UseAutoLinks()
              .UseBootstrap()
              .UseGridTables()
              .UsePipeTables()
              .UseHighlightedCodeBlocks()
              .Build();
        }

        public virtual string ConvertToHtml(string markdown)
        {
            return Markdig.Markdown.ToHtml(Encoding.UTF8.GetString(Encoding.Default.GetBytes(markdown)),
                _markdownPipeline);
        }
    }
}