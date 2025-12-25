using System.Text;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
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
              .UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
              .UseAutoLinks()
              .UseBootstrap()
              .UseGridTables()
              .UsePipeTables()
              .UseHighlightedCodeBlocks()
              .UseAlertBlocks()
              .Build();
        }

        public virtual string ConvertToHtml(string markdown)
        {
            return Markdig.Markdown.ToHtml(Encoding.UTF8.GetString(Encoding.Default.GetBytes(markdown)),
                _markdownPipeline);
        }
    }
}