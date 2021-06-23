using Markdig;

namespace Volo.Docs.Markdown.Extensions
{
    public static class MarkdownPipelineBuilderExtensions
    {
        public static MarkdownPipelineBuilder UseCustomCodeBlock(this MarkdownPipelineBuilder pipeline)
        {
            pipeline.Extensions.AddIfNotAlready<CustomCodeBlockExtension>();
            return pipeline;
        }
    }
}