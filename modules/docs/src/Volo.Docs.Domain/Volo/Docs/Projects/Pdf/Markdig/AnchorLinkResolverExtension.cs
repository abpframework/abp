using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;

namespace Volo.Docs.Projects.Pdf.Markdig;

public class AnchorLinkResolverExtension : IMarkdownExtension
{
    private readonly PdfDocument _document;
    
    public AnchorLinkResolverExtension(PdfDocument document)
    {
        _document = document;
    }
    
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            htmlRenderer.ObjectRenderers.Replace<LinkInlineRenderer>(new AnchorLinkRenderer(_document));
        }
    }
}