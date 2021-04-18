using Markdig;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Public.Web.Renderers
{
    public class MarkdownToHtmlRenderer : IMarkdownToHtmlRenderer, ITransientDependency
    {
        protected MarkdownPipeline MarkdownPipeline { get; }

        public MarkdownToHtmlRenderer(MarkdownPipeline markdownPipeline)
        {
            MarkdownPipeline = markdownPipeline;
        }

        public Task<string> RenderAsync(string rawMarkdown)
        {
            return Task.FromResult(
                Markdown.ToHtml(rawMarkdown, MarkdownPipeline));
        }
    }
}
